using Application.Exceptions;
using Application.Interfaces;
using Application.Models.Dtos.Admin;
using AutoMapper;
using Domain.Entities.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Admin;

public class CreateUserCommand : IRequest<UserOutDto>
{
    public UserInDto? User { get; set; }

    internal sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserOutDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserOutDto> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            string userName = request.User!.UserName;
            var userExists = await _context.Users
                .AnyAsync(u =>
                    u.UserName.ToLower() == userName.ToLower(),
                    cancellationToken);

            if (userExists)
            {
                throw new AlreadyExistsException(
                    name: typeof(User).FullName,
                    property: "UserName",
                    request.User!.UserName);
            }

            var userEntity = _mapper.Map<User>(request.User);
            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<UserOutDto>(userEntity);
        }
    }
}
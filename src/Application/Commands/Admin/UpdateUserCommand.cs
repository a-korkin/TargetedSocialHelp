using Application.Exceptions;
using Application.Interfaces;
using Application.Models.Dtos.Admin;
using AutoMapper;
using Domain.Entities.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Admin;

public class UpdateUserCommand : IRequest<UserOutDto>
{
    public Guid Id { get; set; }
    public UserInDto? UserIn { get; set; }

    internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserOutDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserOutDto> Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (userEntity is null)
                throw new NotFoundException(name: typeof(User).FullName, key: request.Id);

            userEntity = _mapper.Map<User>(request.UserIn);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserOutDto>(userEntity);
        }
    }
}
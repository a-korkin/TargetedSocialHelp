using Application.Exceptions;
using Application.Interfaces;
using Application.Models.Dtos.Admin;
using AutoMapper;
using Domain.Entities.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Admin;

public class GetUserQuery : IRequest<UserOutDto>
{
    public Guid Id { get; set; }

    internal sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserOutDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserOutDto> Handle(
            GetUserQuery request, 
            CancellationToken cancellationToken)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (userEntity is null)
                throw new NotFoundException(name: typeof(User).FullName, key: request.Id);

            return _mapper.Map<UserOutDto>(userEntity);
        }
    }
}
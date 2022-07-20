using Application.Interfaces;
using Application.Models.Dtos.Admin;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Admin;

public class GetUsersQuery : IRequest<IEnumerable<UserOutDto>>
{
    internal sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserOutDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<UserOutDto>> Handle(
            GetUsersQuery request,
            CancellationToken cancellationToken)
        {
            var users = await _context.Users
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<UserOutDto>>(users);
        }
    }
}
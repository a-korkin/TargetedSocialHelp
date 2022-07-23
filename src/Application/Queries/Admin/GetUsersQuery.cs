using Application.Extensions;
using Application.Interfaces;
using Application.Models.Dtos.Admin;
using Application.Models.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Queries.Admin;

public class GetUsersQuery : ResourceParameters, IRequest<PaginatedList<UserOutDto>>
{
    internal sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedList<UserOutDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PaginatedList<UserOutDto>> Handle(
            GetUsersQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Users
                .ProjectTo<UserOutDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
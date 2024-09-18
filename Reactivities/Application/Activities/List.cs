using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence.Entities;

namespace Application.Activities;

public class List
{
    public class Query : IRequest<Result<PagedList<ActivityDto>>>
    {
        public ActivityParams Params { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public Handler(DBContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        public async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Activities
                .Where(a => a.Date >= request.Params.StartDate)
                .OrderBy(d => d.Date)
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,
                    new { currentUsername = _userAccessor.GetUsername() })
                .AsQueryable();

            //The current user is going to activites:
            if (request.Params.IsGoing && !request.Params.IsHost)
            {
                query = query
                    .Where(x => x.Attendees.Any(a => a.Username == _userAccessor.GetUsername()));
            }

            if (!request.Params.IsGoing && request.Params.IsHost)
            {
                query = query
                    .Where(x => x.HostUsername == _userAccessor.GetUsername());
            }

            var activities = await PagedList<ActivityDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize);

            return Result<PagedList<ActivityDto>>.Success(activities);
        }
    }
}
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Application.Activities;

public class Details
{
    public class Query : IRequest<Result<ActivityDto>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<ActivityDto>>
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

        public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,
                        new { currentUsername = _userAccessor.GetUsername() })
                    .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken: cancellationToken)
                ;

            var host = activity!.Attendees
                .First(a => a.Username == activity.HostUsername);

            activity.Attendees = activity.Attendees
                .Where(att => att.Username != activity.HostUsername)
                .Prepend(host)
                .ToList();

            return Result<ActivityDto>.Success(activity);
        }
    }
}
using Application.Activities;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Application.Profiles;

public class ListActivities
{
    public class Command : IRequest<Result<List<UserActivityDto>>>
    {
        public string Predicate { get; set; }
        public string Username { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<List<UserActivityDto>>>
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

        public async Task<Result<List<UserActivityDto>>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.Username);

            if (user is null) return Result<List<UserActivityDto>>.Failure("Username not found.");

            var activitiesQuery = _context.Activities
                    .Where(a => a.Attendees.Any(att => att.AppUserId == user!.Id))
                    .AsQueryable()
                ;
            switch (request.Predicate)
            {
                case "future":
                    activitiesQuery = activitiesQuery
                        .Where(a => a.Date >= DateTime.UtcNow);
                    break;
                case "past":
                    activitiesQuery = activitiesQuery
                        .Where(a => a.Date < DateTime.UtcNow);
                    break;
                case "hosting":
                    activitiesQuery = activitiesQuery
                        .Where(a => a.Date >= DateTime.UtcNow)
                        .Where(a =>
                            a.Attendees.Any(att => att.IsHost && att.AppUserId == user.Id));
                    break;
            }

            var userActivitiesQuery = await activitiesQuery
                    .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider)
                    .ToListAsync()
                ;
            return Result<List<UserActivityDto>>.Success(userActivitiesQuery);
        }
    }
}
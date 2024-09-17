using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Application.Followers;

public class FollowToggle
{
    public class Command : IRequest<Result<Unit>>
    {
        public string TargetUsername { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DBContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DBContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var observer = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());

            var target = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.TargetUsername);

            if (target is null) return null;

            var following = await _context.UserFollowings.FindAsync(observer.Id, target.Id);

            if (following is null)
            {
                following = new UserFollowing()
                {
                    Target = target,
                    Observer = observer
                };

                _context.UserFollowings.Add(following);
            }
            else
            {
                _context.UserFollowings.Remove(following);
            }

            var success = await _context.SaveChangesAsync() > 0;
            
            if (success) return Result<Unit>.Success(Unit.Value);

            return Result<Unit>.Failure("Failed to update following.");
        }
    }
}
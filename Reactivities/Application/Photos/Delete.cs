using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Application.Photos;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public string Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DBContext _context;
        private readonly IPhotoAccessor _photoAccessor;
        private readonly IUserAccessor _userAccessor;

        public Handler(DBContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
        {
            _context = context;
            _photoAccessor = photoAccessor;
            _userAccessor = userAccessor;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername(),
                    cancellationToken: cancellationToken);
            if (user is null) return null;

            var photo = user.Photos.FirstOrDefault(p => p.Id == request.Id);

            if (photo is null) return null;

            if (photo.IsMain) return Result<Unit>.Failure("You cannot delete your main photo");

            var result = await _photoAccessor.DeletePhoto(photo.Id);

            if (result is null) return Result<Unit>.Failure("Problem deleting photo from Cloudinary.");

            user.Photos.Remove(photo);

            var success = await _context.SaveChangesAsync() > 0;
            if (success) return Result<Unit>.Success(Unit.Value);

            return Result<Unit>.Failure("Problem deleting photo from API.");
        }
    }
}
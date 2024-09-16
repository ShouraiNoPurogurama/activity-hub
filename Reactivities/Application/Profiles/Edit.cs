using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Application.Profiles;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.DisplayName).NotEmpty();
        }
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());

            if (user is null) return Result<Unit>.Failure("User not found");

            user.Bio = request.Bio ?? user.Bio;
            user.DisplayName = request.DisplayName ?? user.DisplayName;

            var result = await _context.SaveChangesAsync() > 0;
            if (result) return Result<Unit>.Success(Unit.Value);

            return Result<Unit>.Failure("Error editing user profile");
        }
    }
}
using Application.Core;
using MediatR;
using Persistence.Entities;

namespace Application.Activities;

public class Delete
{
    public class Command : IRequest<Result<Unit>?>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>?>
    {
        private readonly DBContext _context;

        public Handler(DBContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Id);
            
            // if (activity is null)
            // {
            //     return null;
            // }
            
            _context.Remove(activity);
            
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            
            if (!result) return Result<Unit>.Failure("Failed to delete the activity.");
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
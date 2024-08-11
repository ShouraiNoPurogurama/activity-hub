using MediatR;
using Persistence.Entities;

namespace Application.Activities;

public class Delete
{
    public class Command : IRequest
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly DBContext _context;

        public Handler(DBContext context)
        {
            _context = context;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Id);

            if (activity is null)
            {
                throw new InvalidOperationException("Delete failed. Activity not found.");
            }
            
            _context.Remove(activity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
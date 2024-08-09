using Domain;
using MediatR;
using Persistence.Entities;

namespace Application.Activities;

public class Create
{
    public class Command : IRequest
    {
        public Activity Activity { get; set; }
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
            _context.Activities.Add(request.Activity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
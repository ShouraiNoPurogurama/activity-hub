using Domain;
using MediatR;
using Persistence.Entities;

namespace Application.Activities;

public class Details
{
    public class Query : IRequest<Activity>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Activity>
    {
        private readonly DBContext _context;

        public Handler(DBContext context)
        {
            _context = context;
        }

        public async Task<Activity> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Activities.FindAsync(request.Id);
        }
    }
}
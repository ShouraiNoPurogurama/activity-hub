using Application.Core;
using Domain;
using MediatR;
using Persistence.Entities;

namespace Application.Activities;

public class Details
{
    public class Query : IRequest<Result<Activity>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<Activity>>
    {
        private readonly DBContext _context;

        public Handler(DBContext context)
        {
            _context = context;
        }

        public async Task<Result<Activity>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Id);
            return Result<Activity>.Success(activity);
        }
    }
}
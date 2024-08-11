using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Entities;

namespace Application.Activities;

public class List
{
    public class Query : IRequest<List<Activity>>
    {
    }

    public class Handler : IRequestHandler<Query, List<Activity>>
    {
        private readonly DBContext _context;
        private readonly ILogger<List> _logger;

        public Handler(DBContext context, ILogger<List> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
        {
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await Task.Delay(1000, cancellationToken);
                    _logger.LogInformation($"Task {i} has been completed.");
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Task was cancelled");
                throw;
            }
            return await _context.Activities.ToListAsync(cancellationToken );
        }
    }
}
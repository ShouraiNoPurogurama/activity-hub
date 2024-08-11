using AutoMapper;
using Domain;
using MediatR;
using Persistence.Entities;

namespace Application.Activities;

public class Edit
{
    public class Command : IRequest
    {
        public Activity Activity { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private DBContext _context;
        private readonly IMapper _mapper;

        public Handler(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;   
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Activity.Id);
            _mapper.Map(request.Activity, activity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
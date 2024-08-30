using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence.Entities;

namespace Application.Activities;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public Activity Activity { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private DBContext _context;
        private readonly IMapper _mapper;

        public Handler(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Activity).SetValidator(new ActivityValidator());
            }
        }


        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Activity.Id);
            
            _mapper.Map(request.Activity, activity);
            
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            
            if (!result) return Result<Unit>.Failure("Failed to edit the activity.");
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
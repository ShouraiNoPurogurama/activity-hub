﻿using Domain;
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

        public Handler(DBContext context)
        {
            _context = context;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Activity.Id);
            activity.Title = request.Activity.Title ?? activity.Title;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
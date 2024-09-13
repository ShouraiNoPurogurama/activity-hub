﻿using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Application.Activities;

public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public Activity Activity { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(c => c.Activity).SetValidator(new ActivityValidator());
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
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());

            var attendee = new ActivityAttendee()
            {
                AppUser = user,
                Activity = request.Activity,
                IsHost = true
            };
            
            request.Activity.Attendees.Add(attendee);
            
            _context.Activities.Add(request.Activity);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Unit>.Failure("Failed to create activity.");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
﻿using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Application.Activities;

public class List
{
    public class Query : IRequest<Result<List<Activity>>>
    {
    }

    public class Handler : IRequestHandler<Query, Result<List<Activity>>>
    {
        private readonly DBContext _context;

        public Handler(DBContext context)
        {
            _context = context;
        }

        public async Task<Result<List<Activity>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _context.Activities.ToListAsync(cancellationToken);
            return Result<List<Activity>>.Success(result);
        }
    }
}
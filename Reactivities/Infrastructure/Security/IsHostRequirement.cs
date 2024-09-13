using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Infrastructure.Security;

public class IsHostRequirement : IAuthorizationRequirement
{
}

public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
{
    private readonly DBContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IsHostRequirementHandler(DBContext dbContext,
        IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        //Cannot use await keyword bc we want to return an CompletedTask obj
        if (userId is null) return Task.CompletedTask;

        var activityId = Guid.Parse(_httpContextAccessor.HttpContext?.Request.RouteValues
            .SingleOrDefault(x => x.Key == "id").Value
            ?.ToString());

        //After as no tracking, the app will never update the Host attendee record again
        
        //If we track this record, after Auto mapper map the record in Edit controller as Attendee = [], ef will
        //delete the Host attendee record as it can't find the tracked Host attendee in Activity Attendee list.
        var attendee = _dbContext.ActivityAttendees
            .AsNoTracking()
            .SingleOrDefaultAsync(a => a.AppUserId == userId && a.ActivityId == activityId).Result;
        
        
        // var attendee = _dbContext.ActivityAttendees
        //     .FindAsync(userId, activityId).Result;
        // var attendee2 = _dbContext.ActivityAttendees
        //     .FindAsync("2da109e3-e245-4ac8-bdeb-35873244de46", activityId).Result;

        if (attendee is null) return Task.CompletedTask;

        //If a success flag is set at this point, the user will be authorised to go ahead
        if (attendee.IsHost) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
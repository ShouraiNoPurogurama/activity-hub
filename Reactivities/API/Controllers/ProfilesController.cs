using Application.Core;
using Application.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProfilesController : BaseApiController
{
    [HttpGet("{username}")]
    public async Task<IActionResult> GetProfile(string username)
    {
        return HandleResult(await Mediator.Send(new Details.Query() { Username = username }));
    }

    [HttpPut]
    public async Task<IActionResult> EditProfile([FromBody] Edit.Command command)
    {
        return HandleResult(await Mediator.Send(new Edit.Command()
            { Bio = command.Bio, DisplayName = command.DisplayName }));
    }
    
    [HttpGet("{username}/activities")]
    public async Task<IActionResult> GetProfileActivities([FromQuery] string predicate, string username)
    {
        return HandleResult(await Mediator.Send(new ListActivities.Command()
        {
            Username = username,
            Predicate = predicate
        }));
    }

}
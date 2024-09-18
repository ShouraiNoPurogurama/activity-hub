using Application.Activities;
using Application.Core;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetActivities([FromQuery] ActivityParams param,CancellationToken cancellationToken)
        {
            return HandlePagedResult(await Mediator.Send(new List.Query() {Params = param}, cancellationToken));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            var result = await Mediator.Send(new Details.Query { Id = id });

            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Activity activity)
        {
            var result = await Mediator.Send(new Create.Command { Activity = activity });

            return HandleResult(result);
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, [FromBody] Activity activity)
        {
            activity.Id = id;
            await Mediator.Send(new Edit.Command { Activity = activity });
            return Ok();
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await Mediator.Send(new Delete.Command { Id = id });
            return HandleResult(result);
        }

        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(Guid id)
        {
            var result = await Mediator.Send(new UpdateAttendace.Command() { Id = id });
            return HandleResult(result);
        }
    }
}
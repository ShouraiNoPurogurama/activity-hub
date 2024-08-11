using Application.Activities;
using Domain;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetActivities(CancellationToken cancellationToken)
        {
            return await Mediator.Send(new List.Query(), cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity?>> GetActivity(Guid id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Activity activity)
        {
            await Mediator.Send(new Create.Command { Activity = activity });
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Activity activity)
        {
            activity.Id = id;
            await Mediator.Send(new Edit.Command { Activity = activity });
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new Delete.Command { Id = id });
            return Ok();
        }
        
    }
}
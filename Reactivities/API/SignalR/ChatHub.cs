using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

public class ChatHub : Hub
{
    private readonly IMediator _mediator;

    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    //This method name is called in the server side to send new comments
    //case-sensitive is important when calling this method 
    public async Task SendComment(Create.Command command)
    {
        var comment = await _mediator.Send(command);

        await Clients
            .Group(command.ActivityId.ToString())
            .SendAsync("ReceiveComment", comment.Value);
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        //Get the activity id form request query. Ex: http://localhost:5000/chat?activityId=
        var activityId = httpContext!.Request.Query["activityId"];
        await Groups.AddToGroupAsync(Context.ConnectionId, activityId);
        var result = await _mediator.Send(new List.Query() { ActivityId = Guid.Parse(activityId) });
        //Send comment to caller who send the request
        //Client will use hub connection named LoadComments to receive new posted comments
        //Return the comment list of the specified activity through activity ID
        await Clients.Caller.SendAsync("LoadComments", result.Value);
                
    }
}
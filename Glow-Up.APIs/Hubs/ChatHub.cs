using Glow_Up.Core.DTOs.Messages;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace Glow_Up.APIs.Hubs;

public class ChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var userIdStr = httpContext?.Request.Query["userId"].ToString();

        if (int.TryParse(userIdStr, out var userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var httpContext = Context.GetHttpContext();
        var userIdStr = httpContext?.Request.Query["userId"].ToString();

        if (int.TryParse(userIdStr, out var userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId.ToString());
        }

        await base.OnDisconnectedAsync(exception);
    }
}




using Glow_Up.Core.DTOs.Messages;
using Glow_Up.Core.Services.Messages;
using Microsoft.AspNetCore.SignalR;

namespace Glow_Up.APIs.Hubs;

public class MessagePublisher : IMessagePublisher
{
    private readonly IHubContext<ChatHub> _chatHub;

    public MessagePublisher(IHubContext<ChatHub> chatHub)
    {
        _chatHub = chatHub;
    }

    public async Task PublishMessageAsync(MessageDto message)
    {
        await _chatHub.Clients.Group(message.RecipientId.ToString())
            .SendAsync("ReceiveMessage", message);
    }
}

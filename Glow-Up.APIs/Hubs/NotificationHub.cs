using Glow_Up.Core.DTOs.Notifications;
using Glow_Up.Core.Services.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace Glow_Up.APIs.Hubs
{
    public class NotificationHub(INotificationService notificationService) : Hub
    {
        private readonly INotificationService _notificationService = notificationService;

        public async Task SendNotification(NotificationDto notification)
        {
            await Clients.Group(notification.RecipientId.ToString()).SendAsync("ReceiveNotification", notification);
        }

        public async Task JoinGroup(int userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
        }

        public async Task LeaveGroup(int userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId.ToString());
        }
    }
}

using Glow_Up.Core.DTOs.Notifications;
using Glow_Up.Core.Services.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace Glow_Up.APIs.Hubs
{
    public class NotificationPublisher(IHubContext<NotificationHub> notificationHub) : INotificationPublisher
    {

        public async Task PublishNotificationAsync(NotificationDto notification)
        {
            await notificationHub.Clients.Group(notification.RecipientId.ToString())
                .SendAsync("ReceiveNotification", notification);
        }

    }
}

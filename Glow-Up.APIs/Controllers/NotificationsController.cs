using Glow_Up.Core.DTOs.Notifications;
using Glow_Up.Core.Services.Notifications;
using Glow_Up.Services.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Glow_Up.APIs.Controllers
{

    public class NotificationsController(INotificationService notificationService) : ApiBaseController
    {
        private readonly INotificationService _notificationService = notificationService;

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotifications(int userId)
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);
            return Ok(notifications);
        }

        [HttpPut("read/{notificationId}/{userId}")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId, int userId)
        {
            await _notificationService.MarkNotificationAsReadAsync(notificationId, userId);
            return Ok("Notification marked as read.");
        }

       
        [HttpPut("read/{userId}")]
        public async Task<IActionResult> MarkAllNotificationsAsRead(int userId)
        {
            await _notificationService.MarkAllNotificationsAsReadAsync(userId);
            return Ok("Notification marked as read.");
        }
    }
}

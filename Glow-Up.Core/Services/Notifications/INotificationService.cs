using Glow_Up.Core.DTOs.Notifications;
using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Services.Notifications
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(int userId);
        //Task<PagedResult<NotificationDto>> GetNotificationsByUserIdAsync(int userId, int page, int pageSize);
        Task MarkAllNotificationsAsReadAsync(int userId);
        Task MarkNotificationsAsReadAsync(IEnumerable<int> notificationIds, int userId);
        Task MarkNotificationAsReadAsync(int notificationId, int userId);
        Task CreateNotificationAsync(Notification notification);
        Task CreateLikeNotificationAsync(int postId, int userIdLikingPost);
        Task CreateCommentNotificationAsync(int postId, int userIdCommentingPost);
        Task CreateFollowNotificationAsync(int userIdFollower, int userIdFollowing);
    }
}

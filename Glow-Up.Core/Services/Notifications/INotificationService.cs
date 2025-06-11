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
        Task CreateCommentNotificationAsync(int commentId, int userIdCommentingPost);
        Task CreateFollowNotificationAsync(int userIdFollower, int userIdFollowing);
        Task CreateMessageNotificationAsync(int senderId, int recipientId, int messageId);
        Task CreateBHCommentNotificationAsync(int commenterId, int postAuthorId, int postId, int commentId);
        Task CreateBHLikeNotificationAsync(int likerId, int postAuthorId, int postId);
        Task CreateBHVoteNotificationAsync(int voterId, int commentAuthorId, int postId, int commentId, bool isUpvote);
        Task CreateReplyNotificationAsync(int replierId, int commentAuthorId, int postId, int replyId);
        Task CreateShareNotificationAsync(int sharerId, int postAuthorId, int postId);
    }
}

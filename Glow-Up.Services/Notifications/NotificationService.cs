using AutoMapper;
using Glow_Up.Core.DTOs.Notifications;
using Glow_Up.Core.Models;
using Glow_Up.Core.Repositories;
using Glow_Up.Core.Services.Notifications;
using Glow_Up.Core.Specifications.Notification_Spec;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Services.Notifications
{
    public class NotificationService(IUnitOfWork unitOfWork, IMapper mapper, INotificationPublisher notificationPublisher) : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly INotificationPublisher _notificationPublisher = notificationPublisher;

        public async Task CreateNotificationAsync(Notification notification)
        {
            if (notification.Sender == null || notification.Recipient == null)
            {
                throw new Exception("Sender or recipient not found.");
            }

            _unitOfWork.Repository<Notification>().Add(notification);

            await _unitOfWork.CompleteAsync();

            var notificationDto = new NotificationDto
            {
                Id = notification.Id,
                RecipientId = notification.RecipientId,
                SenderId = notification.SenderId,
                Type = notification.Type,
                TargetId = notification.TargetId,
                CreatedAt = notification.CreatedAt,
                IsRead = notification.IsRead,
                Sender = new SenderDto
                {
                    FirstName = notification.Sender.FirstName,
                    LastName = notification.Sender.LastName,
                    ProfilePic = notification.Sender.ProfilePic
                }
            };

            await _notificationPublisher.PublishNotificationAsync(notificationDto);
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(int userId)
        {
            var spec = new NotificationByUserIdSpecification(userId);

            var notifications = await _unitOfWork.Repository<Notification>().GetAllWithSpecAsync(spec);

            var dto = notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                RecipientId = n.RecipientId,
                SenderId = n.SenderId,
                Type = n.Type,
                TargetId = n.TargetId,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead,
                Sender = new SenderDto
                {
                    Id = n.Sender.Id,
                    FirstName = n.Sender.FirstName,
                    LastName = n.Sender.LastName,
                    ProfilePic = n.Sender.ProfilePic
                }
            });

            return dto;
        }

        public Task MarkAllNotificationsAsReadAsync(int userId)
        {
            var spec = new UnReadNotificationByUserIdSpecification(userId);

            var notifications = _unitOfWork.Repository<Notification>().GetAllAsQueryable(spec);

            foreach (var item in notifications)
            {
                item.IsRead = true;
            }

            return _unitOfWork.CompleteAsync();
        }

        public async Task MarkNotificationsAsReadAsync(IEnumerable<int> notificationIds, int userId)
        {
            var notifications = _unitOfWork.Repository<Notification>().GetAllAsQueryable();

            var notificationsToUpdate = await notifications.Where(n =>
                notificationIds.Contains(n.Id) && n.RecipientId == userId).ToListAsync();

            foreach (var notification in notificationsToUpdate)
            {
                notification.IsRead = true;
            }

           await _unitOfWork.CompleteAsync();
        }

        public async Task MarkNotificationAsReadAsync(int notificationId, int userId)
        {
            var spec = new NotificationByUserIdSpecification(userId);

            var notificationsByUser = await _unitOfWork.Repository<Notification>().GetAllWithSpecAsync(spec);

            var notification = notificationsByUser.FirstOrDefault(n => n.Id == notificationId) ?? throw new Exception("Notification not found.");

            notification.IsRead = true;

            _unitOfWork.Repository<Notification>().Update(notification);

            await _unitOfWork.CompleteAsync();

        }

        public async Task CreateLikeNotificationAsync(int postId, int userIdLikingPost)
        {
            var post = await _unitOfWork.Repository<Post>().GetByIdAsync(postId) ?? throw new Exception("Post not found.");

            if (post.UserId == userIdLikingPost)
            {
                return;
            }

            var notification = new Notification
            {
                RecipientId = post.UserId,
                SenderId = userIdLikingPost,
                Sender = await _unitOfWork.Repository<User>().GetByIdAsync(userIdLikingPost),
                Recipient = await _unitOfWork.Repository<User>().GetByIdAsync(post.UserId),
                Type = "like",
                TargetId = postId,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await CreateNotificationAsync(notification);
        }

        public async Task CreateCommentNotificationAsync(int commentId, int userIdCommenting)
        {
            var comment = await _unitOfWork.Repository<Comment>().GetByIdAsync(commentId) ?? throw new Exception("Comment not found.");

            if (comment.UserId == comment.Post.UserId)
            {
                return;
            }

            var notification = new Notification
            {
                RecipientId = comment.Post.UserId,
                SenderId = userIdCommenting,
                Sender = await _unitOfWork.Repository<User>().GetByIdAsync(userIdCommenting),
                Recipient = await _unitOfWork.Repository<User>().GetByIdAsync(comment.Post.UserId),
                Type = "comment",
                TargetId = commentId,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await CreateNotificationAsync(notification);
        }

        public async Task CreateFollowNotificationAsync(int followerId, int followingId)
        {
            // Don't create a notification if a user tries to follow themselves
            if (followerId == followingId)
            {
                return;
            }

            var notification = new Notification
            {
                RecipientId = followingId,
                SenderId = followerId,
                Sender = await _unitOfWork.Repository<User>().GetByIdAsync(followerId),
                Recipient = await _unitOfWork.Repository<User>().GetByIdAsync(followingId),
                Type = "follow",
                TargetId = null,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await CreateNotificationAsync(notification);
        }
    }
}

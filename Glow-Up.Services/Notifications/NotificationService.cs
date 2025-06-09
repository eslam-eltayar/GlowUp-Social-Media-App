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


        public async Task CreateReplyNotificationAsync(int replierId, int commentAuthorId, int postId, int replyId)
        {
            // Don't create a notification if someone replies to their own comment
            if (replierId == commentAuthorId)
            {
                return;
            }

            var replier = await _unitOfWork.Repository<User>().GetByIdAsync(replierId);
            var recipient = await _unitOfWork.Repository<User>().GetByIdAsync(commentAuthorId);

            if (replier == null || recipient == null)
            {
                throw new Exception("User not found");
            }

            var notification = new Notification
            {
                SenderId = replierId,
                RecipientId = commentAuthorId,
                Type = "reply",
                TargetId = postId,
                SubTargetId = replyId,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                Sender = replier,
                Recipient = recipient
            };

            await CreateNotificationAsync(notification);
        }

        public async Task CreateMessageNotificationAsync(int senderId, int recipientId, int messageId)
        {
            // Don't create a notification if someone messages themselves
            if (senderId == recipientId)
            {
                return;
            }

            var sender = await _unitOfWork.Repository<User>().GetByIdAsync(senderId);
            var recipient = await _unitOfWork.Repository<User>().GetByIdAsync(recipientId);

            if (sender == null || recipient == null)
            {
                throw new Exception("Sender or recipient not found.");
            }

            var notification = new Notification
            {
                RecipientId = recipientId,
                SenderId = senderId,
                Sender = sender,
                Recipient = recipient,
                Type = "message",
                TargetId = messageId,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await CreateNotificationAsync(notification);
        }

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

        public async Task CreateBHCommentNotificationAsync(int commenterId, int postAuthorId, int postId, int commentId)
        {
            if (commenterId == postAuthorId) return; // Don't notify self

            var commenter = await _unitOfWork.Repository<User>().GetByIdAsync(commenterId);
            var recipient = await _unitOfWork.Repository<User>().GetByIdAsync(postAuthorId);

            if (commenter == null || recipient == null)
                throw new Exception("User not found");

            var notification = new Notification
            {
                SenderId = commenterId,
                RecipientId = postAuthorId,
                Type ="BHComment",
                TargetId = postId,
                SubTargetId = commentId,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                Sender = commenter,
                Recipient = recipient
            };

            await CreateNotificationAsync(notification);
        }

        public async Task CreateBHLikeNotificationAsync(int likerId, int postAuthorId, int postId)
        {
            if (likerId == postAuthorId) return;

            var liker = await _unitOfWork.Repository<User>().GetByIdAsync(likerId);
            var recipient = await _unitOfWork.Repository<User>().GetByIdAsync(postAuthorId);

            if (liker == null || recipient == null)
                throw new Exception("User not found");

            var notification = new Notification
            {
                SenderId = likerId,
                RecipientId = postAuthorId,
                Type = "BHLike",
                TargetId = postId,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                Sender = liker,
                Recipient = recipient
            };

            await CreateNotificationAsync(notification);
        }

        public async Task CreateBHVoteNotificationAsync(int voterId, int commentAuthorId, int postId, int commentId, bool isUpvote)
        {
            if (voterId == commentAuthorId) return;

            var voter = await _unitOfWork.Repository<User>().GetByIdAsync(voterId);
            var recipient = await _unitOfWork.Repository<User>().GetByIdAsync(commentAuthorId);

            if (voter == null || recipient == null)
                throw new Exception("User not found");

            var notification = new Notification
            {
                SenderId = voterId,
                RecipientId = commentAuthorId,
                Type = "BHVote",
                TargetId = postId,
                SubTargetId = commentId,
                AdditionalInfo = isUpvote ? "upvote" : "downvote",
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                Sender = voter,
                Recipient = recipient
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

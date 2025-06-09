using Glow_Up.Core.DTOs.Comment;
using Glow_Up.Core.Models;
using Glow_Up.Core.Repositories;
using Glow_Up.Core.Services.Comment;
using Glow_Up.Core.Services.Files;
using Glow_Up.Core.Services.Notifications;
using Glow_Up.Core.Specifications.Comment_Spec;
using Glow_Up.Services.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INotificationService _notificationService;

        public CommentService(
            IFileUploadService fileUploadService,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment webHostEnvironment,
            INotificationService notificationService)
        {
            _fileUploadService = fileUploadService;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _notificationService = notificationService;
        }

        public async Task<CommentToReturnDto> CreateCommentAsync(int userId, int postId, AddCommentDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Input cannot be empty.");

            if (string.IsNullOrEmpty(dto.Text) && dto.Media == null)
                throw new ArgumentException("A comment must have either text, an image, or both.");


            var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);

            if (user == null)
                throw new Exception($"User not found with ID: {userId}");


            var post = await _unitOfWork.Repository<Post>().GetByIdAsync(postId);

            if (post == null)
                throw new Exception($"Post not found with ID: {postId}");


            string mediaUrl = null;

            if (dto.Media != null && dto.Media.Length > 0)
            {
                mediaUrl = await _fileUploadService.UploadFileAsync(dto.Media, "comments");
            }

            var comment = new Comment
            {
                Text = dto.Text,
                MediaUrl = mediaUrl,
                UserId = userId,
                PostId = postId,
                CreatedAt = DateTime.UtcNow,
            };

            _unitOfWork.Repository<Comment>().Add(comment);

            int saveResult = await _unitOfWork.CompleteAsync();

            if (saveResult <= 0)
                throw new Exception("An error occurred while saving the comment.");

            return new CommentToReturnDto
            {
                Id = comment.Id,
                Text = comment.Text,
                MediaUrl = comment.MediaUrl,
                UserId = comment.UserId,
                PostId = comment.PostId,
                CreatedAt = "Just Now"
            };
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            if (commentId <= 0)
                throw new Exception("Invalid workshop Id");

            var comment = await _unitOfWork.Repository<Comment>().GetByIdAsync(commentId);

            if (comment == null)
                throw new Exception("Comment not founded");

            if (!string.IsNullOrEmpty(comment.MediaUrl))
            {

                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "comments", comment.MediaUrl);

                imagePath = $"wwwroot{imagePath}";

                if (File.Exists(imagePath))
                    File.Delete(imagePath);

            }

            _unitOfWork.Repository<Comment>().Delete(comment);

            int result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                throw new Exception("There's an error while Deleting Workshop ya Lifaaa");

            return true;
        }

        //public async Task<IReadOnlyList<CommentToReturnDto>> GetCommentsForPostAsync(int postId)
        //{
        //    var spec = new CommentsForPostSpecification(postId);

        //    var comments = await _unitOfWork.Repository<Comment>().GetAllWithSpecAsync(spec);

        //    var commentDtos = new List<CommentToReturnDto>();

        //    foreach (var comment in comments)
        //    {
        //        var dto = new CommentToReturnDto
        //        {
        //            Id = comment.Id,
        //            Text = comment.Text,
        //            MediaUrl = comment.MediaUrl,
        //            UserId = comment.UserId,
        //            PostId = comment.PostId,
        //            ParentCommentId = comment.ParentCommentId,
        //            CreatedAt = Helper.FormatDate(comment.CreatedAt),
        //            UserName = $"{comment.User.FirstName} {comment.User.LastName}",
        //            UserImage = comment.User.ProfilePic,
        //            Replies = new List<CommentToReturnDto>()
        //        };

        //        if (comment.Replies != null)
        //        {
        //            foreach (var reply in comment.Replies)
        //            {
        //                var replyDto = new CommentToReturnDto
        //                {
        //                    Id = reply.Id,
        //                    Text = reply.Text,
        //                    MediaUrl = reply.MediaUrl,
        //                    UserId = reply.UserId,
        //                    PostId = reply.PostId,
        //                    ParentCommentId = reply.ParentCommentId,
        //                    CreatedAt = Helper.FormatDate(reply.CreatedAt),
        //                    UserName = reply.User != null ? $"{reply.User.FirstName} {reply.User.LastName}" : "Unknown",
        //                    UserImage = reply.User?.ProfilePic ?? "",
        //                    Replies = new List<CommentToReturnDto>()
        //                };

        //                dto.Replies.Add(replyDto);
        //            }
        //        }

        //        commentDtos.Add(dto);

        //    }
        //    return commentDtos;
        //}

        //public async Task<CommentToReturnDto> ReplyToCommentAsync(int userId, int postId, CreateReplyDto dto)
        //{
        //    if (dto == null)
        //        throw new Exception("Input cannot be empty.");

        //    if (string.IsNullOrEmpty(dto.Text) && dto.Media == null)
        //        throw new Exception("A reply must have either text, an image, or both.");

        //    var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);

        //    if (user == null)
        //        throw new Exception($"User not found with ID: {userId}");


        //    var post = await _unitOfWork.Repository<Post>().GetByIdAsync(postId);
        //    if (post == null)
        //        throw new Exception($"Post not found with ID: {postId}");


        //    var parentComment = await _unitOfWork.Repository<Comment>().GetByIdAsync(dto.ParentCommentId);

        //    if (parentComment == null)
        //        throw new Exception($"Parent comment not found with ID: {dto.ParentCommentId}");


        //    string mediaUrl = null;

        //    if (dto.Media != null && dto.Media.Length > 0)
        //    {
        //        mediaUrl = await _fileUploadService.UploadFileAsync(dto.Media, "comments");
        //    }


        //    var reply = new Comment
        //    {
        //        Text = dto.Text,
        //        MediaUrl = mediaUrl,
        //        UserId = userId,
        //        PostId = postId,
        //        ParentCommentId = dto.ParentCommentId,
        //        CreatedAt = DateTime.UtcNow,
        //    };

        //    _unitOfWork.Repository<Comment>().Add(reply);

        //    int saveResult = await _unitOfWork.CompleteAsync();

        //    if (saveResult <= 0)
        //        throw new Exception("An error occurred while saving the reply.");


        //    return new CommentToReturnDto
        //    {
        //        Id = reply.Id,
        //        Text = reply.Text,
        //        MediaUrl = reply.MediaUrl,
        //        UserId = reply.UserId,
        //        PostId = reply.PostId,
        //        ParentCommentId = reply.ParentCommentId,
        //        CreatedAt = "Just Now"
        //    };

        //}

        public async Task<CommentToReturnDto> ReplyToCommentAsync(int userId, int postId, CreateReplyDto dto)
        {
            if (dto == null)
                throw new Exception("Input cannot be empty.");

            if (string.IsNullOrEmpty(dto.Text) && dto.Media == null)
                throw new Exception("A reply must have either text, an image, or both.");

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);
            if (user == null)
                throw new Exception($"User not found with ID: {userId}");

            var post = await _unitOfWork.Repository<Post>().GetByIdAsync(postId);
            if (post == null)
                throw new Exception($"Post not found with ID: {postId}");

            // Check parent comment
            var parentComment = await _unitOfWork.Repository<Comment>().GetByIdAsync(dto.ParentCommentId);
            if (parentComment == null)
                throw new Exception($"Parent comment not found with ID: {dto.ParentCommentId}");

            // Check parent reply if specified
            Comment? parentReply = null;
            if (dto.ParentReplyId.HasValue)
            {
                parentReply = await _unitOfWork.Repository<Comment>().GetByIdAsync(dto.ParentReplyId.Value);
                if (parentReply == null)
                    throw new Exception($"Parent reply not found with ID: {dto.ParentReplyId}");

                // Verify that the parent reply belongs to the same comment thread
                if (parentReply.ParentCommentId != dto.ParentCommentId)
                    throw new Exception("Invalid parent reply: reply does not belong to the specified comment thread");
            }

            string? mediaUrl = null;
            if (dto.Media != null && dto.Media.Length > 0)
            {
                mediaUrl = await _fileUploadService.UploadFileAsync(dto.Media, "comments");
            }

            var reply = new Comment
            {
                Text = dto.Text,
                MediaUrl = mediaUrl,
                UserId = userId,
                PostId = postId,
                ParentCommentId = dto.ParentCommentId,
                ParentReplyId = dto.ParentReplyId,  // Add this property to your Comment model
                CreatedAt = DateTime.UtcNow,
            };

            _unitOfWork.Repository<Comment>().Add(reply);
            int saveResult = await _unitOfWork.CompleteAsync();

            if (saveResult <= 0)
                throw new Exception("An error occurred while saving the reply.");

            // Create notification for the parent comment/reply owner
            int notificationRecipientId = dto.ParentReplyId.HasValue ?
                parentReply!.UserId : parentComment.UserId;

            await _notificationService.CreateReplyNotificationAsync(userId, notificationRecipientId, postId, reply.Id);

            return new CommentToReturnDto
            {
                Id = reply.Id,
                Text = reply.Text,
                MediaUrl = reply.MediaUrl,
                UserId = reply.UserId,
                PostId = reply.PostId,
                ParentCommentId = reply.ParentCommentId,
                ParentReplyId = reply.ParentReplyId,
                CreatedAt = "Just Now",
                UserName = $"{user.FirstName} {user.LastName}",
                UserImage = user.ProfilePic,
                Replies = new List<CommentToReturnDto>()
            };
        }

        private CommentToReturnDto MapCommentToDto(Comment comment)
        {
            var dto = new CommentToReturnDto
            {
                Id = comment.Id,
                Text = comment.Text,
                MediaUrl = comment.MediaUrl,
                UserId = comment.UserId,
                PostId = comment.PostId,
                ParentCommentId = comment.ParentCommentId,
                CreatedAt = Helper.FormatDate(comment.CreatedAt),
                UserName = $"{comment.User.FirstName} {comment.User.LastName}",
                UserImage = comment.User.ProfilePic,
            };

            if (comment.Replies != null && comment.Replies.Any())
            {
                dto.Replies = comment.Replies.Select(MapCommentToDto).ToList();
            }

            return dto;
        }
        private List<CommentToReturnDto> BuildCommentHierarchy(IEnumerable<Comment> comments)
        {
            var commentDtos = new Dictionary<int, CommentToReturnDto>();
            var rootComments = new List<CommentToReturnDto>();

            foreach (var comment in comments)
            {
                var dto = MapCommentToDto(comment);
                commentDtos[comment.Id] = dto;

                if (!comment.ParentCommentId.HasValue)
                {
                    rootComments.Add(dto);
                }
                else
                {
                    var parentComment = commentDtos[comment.ParentCommentId.Value];
                    if (comment.ParentReplyId.HasValue)
                    {
                        // Find the parent reply and add this reply to its replies
                        FindAndAddReply(parentComment.Replies, comment.ParentReplyId.Value, dto);
                    }
                    else
                    {
                        // Direct reply to comment
                        parentComment.Replies.Add(dto);
                    }
                }
            }

            return rootComments;
        }

        private void FindAndAddReply(List<CommentToReturnDto> replies, int parentReplyId, CommentToReturnDto newReply)
        {
            foreach (var reply in replies)
            {
                if (reply.Id == parentReplyId)
                {
                    reply.Replies.Add(newReply);
                    return;
                }
                if (reply.Replies.Any())
                {
                    FindAndAddReply(reply.Replies, parentReplyId, newReply);
                }
            }
        }

        public async Task<IReadOnlyList<CommentToReturnDto>> GetCommentsForPostAsync(int postId)
        {
            var spec = new CommentsForPostSpecification(postId);
            var comments = await _unitOfWork.Repository<Comment>().GetAllWithSpecAsync(spec);

            if (!comments.Any())
                return new List<CommentToReturnDto>();

            return BuildCommentHierarchy(comments).AsReadOnly();
        }
    }
}

using Glow_Up.Core.DTOs.BHComment;
using Glow_Up.Core.DTOs.BHPost;
using Glow_Up.Core.DTOs.Post;
using Glow_Up.Core.Enums;
using Glow_Up.Core.Models;
using Glow_Up.Core.Models.BlackHat;
using Glow_Up.Core.Repositories;
using Glow_Up.Core.Services.BlackHat;
using Glow_Up.Core.Services.Files;
using Glow_Up.Core.Specifications.BlackHat;
using Glow_Up.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Services.BlackHat
{
    public class BlackHatService(
        IUnitOfWork unitOfWork,
        IFileUploadService fileUploadService) : IBlackHatService

    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFileUploadService _fileUploadService = fileUploadService;

        public async Task<BHCommentToReturnDto> AddCommentAsync(int postId, CreateBHCommentDto dto, CancellationToken cancellationToken = default)
        {
            var post = await _unitOfWork.Repository<BHPost>().GetByIdAsync(postId);

            if (post == null)
                throw new Exception("Post Not Founded");

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(dto.UserId);

            if (user == null)
                throw new Exception($"User not found with ID: {dto.UserId}");

            if (string.IsNullOrEmpty(dto.Text))
                throw new ArgumentException("A comment cannot be empty!");

            var BHcomment = new BHComment
            {
                UserId = dto.UserId,
                Text = dto.Text,
                BHPostId = postId,
                CreatedAt = DateTime.UtcNow,

            };

            _unitOfWork.Repository<BHComment>().Add(BHcomment);

            int result = await _unitOfWork.CompleteAsync(cancellationToken);

            if (result <= 0)
                throw new Exception("There's an Error while Adding Comment!");

            return new BHCommentToReturnDto
            {
                CommentId = BHcomment.Id,
                PostId = postId,
                Text = BHcomment.Text,
                UserId = dto.UserId,
                VoteCount = BHcomment.VoteCount,

            };

        }

        public async Task<BHPostToReturnDto> CreatePostAsync(int userId, CreateBHPostDto dto, CancellationToken cancellationToken = default)
        {
            if (dto == null) throw new Exception("Input cannot be empty.");

            if (dto.Caption == null && !dto.MediaFiles.Any())
                throw new Exception("The post cannot be Empty!");

            if (!Enum.TryParse<Category>(dto.Category, true, out var parsedCategory))
                throw new Exception($"Invalid category: {dto.Category}");

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId, cancellationToken);

            if (user == null)
                throw new Exception($"User Not founded with this Id {userId}");

            var bHpost = new BHPost
            {
                Caption = dto.Caption,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Category = parsedCategory,
            };

            _unitOfWork.Repository<BHPost>().Add(bHpost);

            int postSaveResult = await _unitOfWork.CompleteAsync(cancellationToken);

            if (postSaveResult <= 0)
                throw new Exception("An error occurred while saving the post.");

            // Save media files

            var mediaItems = new List<BHMedia>();
            var mediaUrls = new List<string>();

            if (dto.MediaFiles.Any())
            {
                foreach (var file in dto.MediaFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileUrl = await _fileUploadService.UploadFileAsync(file, "BHposts");

                        var bHmedia = new BHMedia
                        {
                            Url = fileUrl,
                            Type = Helper.GetMediaType(file.ContentType),
                            CreatedAt = DateTime.Now,
                            BHPostId = bHpost.Id,

                        };

                        mediaItems.Add(bHmedia);
                        mediaUrls.Add(fileUrl);
                    }
                }

                await _unitOfWork.Repository<BHMedia>().AddRange(mediaItems);
                int mediaSaveResult = await _unitOfWork.CompleteAsync(cancellationToken);

                if (mediaSaveResult <= 0)
                    throw new Exception("An error occurred while saving media files.");

            }

            return new BHPostToReturnDto
            {
                PostId = bHpost.Id,
                Caption = bHpost.Caption,
                FilesUrls = mediaUrls,
            };


        }

        public async Task<bool> DecreaseCommentAsync(int commentId, int userId, CancellationToken cancellationToken = default)
        {
            if (commentId <= 0)
                throw new ArgumentException("Invalid comment ID", nameof(commentId));

            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));


            var comment = await _unitOfWork.Repository<BHComment>()
                                            .GetByIdAsync(commentId, cancellationToken)
                                            ?? throw new Exception($"Comment with ID {commentId} not found");


            var existingVote = await _unitOfWork.Repository<CommentVoteHistory>()
                                                .FirstOrDefaultAsync(x => x.BHCommentId == commentId && x.UserId == userId, cancellationToken);


            if (existingVote == null)
                throw new InvalidOperationException("You haven't voted for this comment yet");


            _unitOfWork.Repository<CommentVoteHistory>().Delete(existingVote);

            comment.VoteCount -= 1;

            _unitOfWork.Repository<BHComment>().Update(comment);

            var result = await _unitOfWork.CompleteAsync(cancellationToken);

            if (result <= 0)
                throw new Exception("An Error While Decreasing Comment.");


            return true;
        }

        public async Task<IReadOnlyList<BHCommentToReturnDto>> GetAllCommentsAsync(int postId, CancellationToken cancellationToken = default)
        {
            var spec = new BHCommentsForPost(postId);

            var comments = await _unitOfWork.Repository<BHComment>().GetAllWithSpecAsync(spec);

            if (comments == null || !comments.Any())
                throw new Exception("Not Comments Founded!");

            return comments.Select(c => new BHCommentToReturnDto
            {
                PostId = c.BHPostId,
                CommentId = c.Id,
                Text = c.Text ?? string.Empty,
                UserId = c.UserId,
                VoteCount = c.VoteCount,

            }).ToList().AsReadOnly();

        }

        public async Task<IReadOnlyList<BHPostToReturnDto>> GetAllPostsAsync(string? category, CancellationToken cancellationToken = default)
        {
            var spec = new GetBHPostsSpecification(category);

            var posts = await _unitOfWork.Repository<BHPost>().GetAllWithSpecAsync(spec, cancellationToken);

            if (posts == null || !posts.Any())
                throw new Exception("No Posts founded");

            return posts.Select(post => new BHPostToReturnDto
            {
                PostId = post.Id,
                Caption = post.Caption,
                Categoty = post.Category.ToString(),
                Date = Helper.FormatDate(post.CreatedAt),
                CommentsCount = post.Comments?.Count ?? 0,
                LikesCount = post.Likes?.Count ?? 0,
                FilesUrls = post.Medias?.Select(m => m.Url).ToList() ?? new List<string>(),
                UserId = post.UserId,

            }).ToList().AsReadOnly();


        }

        public async Task<bool> IncreaseCommentAsync(int commentId, int userId, CancellationToken cancellationToken = default)
        {
            if (commentId <= 0)
                throw new ArgumentException("Invalid comment ID", nameof(commentId));

            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            var comment = await _unitOfWork.Repository<BHComment>()
                                            .GetByIdAsync(commentId, cancellationToken)
                                            ?? throw new Exception($"Comment with ID {commentId} not found");

            var existingVote = await _unitOfWork.Repository<CommentVoteHistory>()
                                                .FirstOrDefaultAsync(x => x.BHCommentId == commentId && x.UserId == userId, cancellationToken);

            if (existingVote != null)
                throw new InvalidOperationException("You have already voted for this comment");

            var voteHistory = new CommentVoteHistory
            {
                BHCommentId = commentId,
                UserId = userId
            };

            comment.VoteCount += 1;

            _unitOfWork.Repository<CommentVoteHistory>().Add(voteHistory);
            _unitOfWork.Repository<BHComment>().Update(comment);

            int result = await _unitOfWork.CompleteAsync(cancellationToken);

            if (result <= 0)
                throw new Exception("There's an Error while Increasing Comment");

            return true;
        }

        public async Task<bool> LikePostAsync(int postId, int userId, CancellationToken cancellationToken = default)
        {
            if (postId <= 0)
                throw new ArgumentException("Invalid Post ID", nameof(postId));

            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            var post = await _unitOfWork.Repository<BHPost>()
                                            .GetByIdAsync(postId, cancellationToken)
                                            ?? throw new Exception($"Post with ID {postId} not found");

            var existingLike = await _unitOfWork.Repository<BHLike>()
                                                .FirstOrDefaultAsync(x => x.BHPostId == postId && x.UserId == userId, cancellationToken);

            if (existingLike != null)
                throw new InvalidOperationException("You have already Liked this post");

            var like = new BHLike
            {
                UserId = userId,
                BHPostId = postId,
            };

            _unitOfWork.Repository<BHLike>().Add(like);

            int result = await _unitOfWork.CompleteAsync(cancellationToken);

            if (result <= 0) throw new Exception("There's an Error while Like Post");

            return true;
        }

        public async Task<bool> UnLikePostAsync(int postId, int userId, CancellationToken cancellationToken = default)
        {
            if (postId <= 0)
                throw new ArgumentException("Invalid Post ID", nameof(postId));

            if (userId <= 0)
                throw new ArgumentException("Invalid User ID", nameof(userId));

            var post = await _unitOfWork.Repository<BHPost>()
                                        .GetByIdAsync(postId, cancellationToken)
                                        ?? throw new Exception($"Post with ID {postId} not found");

            var existingLike = await _unitOfWork.Repository<BHLike>()
                                                .FirstOrDefaultAsync(x => x.BHPostId == postId && x.UserId == userId, cancellationToken);

            if (existingLike == null)
                throw new InvalidOperationException("You have not liked this post yet");

            _unitOfWork.Repository<BHLike>().Delete(existingLike);

            int result = await _unitOfWork.CompleteAsync(cancellationToken);

            if (result <= 0) throw new Exception("There's an error while unliking the post");

            return true;
        }

        public async Task<bool> HasUserLikedPostAsync(int postId, int userId, CancellationToken cancellationToken = default)
        {
            if (postId <= 0)
                throw new ArgumentException("Invalid Post ID", nameof(postId));

            if (userId <= 0)
                throw new ArgumentException("Invalid User ID", nameof(userId));

            var existingLike = await _unitOfWork.Repository<BHLike>()
                                                .FirstOrDefaultAsync(x => x.BHPostId == postId && x.UserId == userId, cancellationToken);

            return existingLike != null;
        }

        public async Task<BHPostToReturnDto> GetMostPopularPostAsync(CancellationToken cancellationToken = default)
        {
            var today = DateTime.UtcNow.Date;

            var spec = new GetBHPostsSpecification(null)
            {
                Criteria = post => post.Comments.Any(comment => comment.CreatedAt.Date == today)
            };

            var posts = await _unitOfWork.Repository<BHPost>().GetAllWithSpecAsync(spec, cancellationToken);

            if (posts == null || !posts.Any())
                throw new Exception("No posts with comments found for today");

            var mostPopularPost = posts
                .OrderByDescending(post => post.Comments.Count(comment => comment.CreatedAt.Date == today))
                .FirstOrDefault();

            if (mostPopularPost == null)
                throw new Exception("No posts with comments found for today");

            return new BHPostToReturnDto
            {
                PostId = mostPopularPost.Id,
                Caption = mostPopularPost.Caption,
                Categoty = mostPopularPost.Category.ToString(),
                Date = Helper.FormatDate(mostPopularPost.CreatedAt),
                CommentsCount = mostPopularPost.Comments.Count(comment => comment.CreatedAt.Date == today),
                LikesCount = mostPopularPost.Likes?.Count ?? 0,
                FilesUrls = mostPopularPost.Medias?.Select(m => m.Url).ToList() ?? new List<string>(),
                UserId = mostPopularPost.UserId,
            };
        }

        public async Task<BHPostToReturnDto> GetMostPostReactedThisWeekAsync(CancellationToken cancellationToken = default)
        {
            var startOfWeek = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.Date.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            var spec = new GetBHPostsSpecification(null)
            {
                Criteria = post => post.Likes.Any(like => like.CreatedAt >= startOfWeek && like.CreatedAt < endOfWeek)
            };

            var posts = await _unitOfWork.Repository<BHPost>().GetAllWithSpecAsync(spec, cancellationToken);

            if (posts == null || !posts.Any())
                throw new Exception("No posts with reactions found for this week");

            var mostReactedPost = posts
                .OrderByDescending(post => post.Likes.Count(like => like.CreatedAt >= startOfWeek && like.CreatedAt < endOfWeek))
                .FirstOrDefault();

            if (mostReactedPost == null)
                throw new Exception("No posts with reactions found for this week");

            return new BHPostToReturnDto
            {
                PostId = mostReactedPost.Id,
                Caption = mostReactedPost.Caption,
                Categoty = mostReactedPost.Category.ToString(),
                Date = Helper.FormatDate(mostReactedPost.CreatedAt),
                CommentsCount = mostReactedPost.Comments?.Count ?? 0,
                LikesCount = mostReactedPost.Likes.Count(like => like.CreatedAt >= startOfWeek && like.CreatedAt < endOfWeek),
                FilesUrls = mostReactedPost.Medias?.Select(m => m.Url).ToList() ?? new List<string>(),
                UserId = mostReactedPost.UserId,
            };
        }
    }
}

using Glow_Up.Core.DTOs.Post;
using Glow_Up.Core.Enums;
using Glow_Up.Core.Models;
using Glow_Up.Core.Repositories;
using Glow_Up.Core.Services.Files;
using Glow_Up.Core.Services.Logs;
using Glow_Up.Core.Services.Notifications;
using Glow_Up.Core.Services.Posts;
using Glow_Up.Core.Specifications.Comment_Spec;
using Glow_Up.Core.Specifications.FavPosts_Spec;
using Glow_Up.Core.Specifications.Post_Spec;
using Glow_Up.Core.Specifications.SharedPosts_Spec;
using Glow_Up.Services.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Glow_Up.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INotificationService _notificationService;
        private readonly IActivityLogService _activityLogService;

        public PostService(
            IFileUploadService fileUploadService,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment webHostEnvironment,
            INotificationService notificationService,
            IActivityLogService activityLogService)
        {
            _fileUploadService = fileUploadService;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _notificationService = notificationService;
            _activityLogService = activityLogService;
        }


        public async Task<bool> AddReactionAsync(int postId, AddReactionDto dto)
        {
            var post = await _unitOfWork.Repository<Post>().GetByIdAsync(postId);

            if (post == null)
            {
                throw new Exception("Post Not founded!");
            }

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(dto.UserId);

            if (user == null)
                throw new Exception($"User Not founded with this Id {dto.UserId}");


            if (!Enum.TryParse(dto.ReactType, out ReactType reactionType))
            {
                throw new Exception("Invalid reaction type.");
            }

            // Check if the user has already reacted to the post

            var existingReaction = await _unitOfWork.Repository<Reaction>()
                .FirstOrDefaultAsync(r => r.PostId == postId && r.UserId == dto.UserId);

            if (existingReaction != null)
            {

                existingReaction.Type = reactionType;
                _unitOfWork.Repository<Reaction>().Update(existingReaction);
            }
            else
            {
                var reaction = new Reaction
                {
                    PostId = postId,
                    UserId = dto.UserId,
                    Type = reactionType
                };

                _unitOfWork.Repository<Reaction>().Add(reaction);
            }

            // Save changes to the database

            int result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
            {
                throw new Exception("An error occurred while adding the reaction.");
            }

            await _notificationService.CreateLikeNotificationAsync(postId, dto.UserId);

            await _activityLogService.LogActivityAsync(
                dto.UserId,
                ActivityType.Like,
                postId,
                dto.ReactType
             );

            return true;

        }

        public async Task<PostToReturnDto> CreateNewPostAsync(int userId, CreatePostDto dto)
        {
            if (dto == null) throw new Exception("Input cannot be empty.");

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);
            if (user == null)
                throw new Exception($"User not found with this Id {userId}");

            var post = new Post
            {
                Caption = dto.Caption,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                PostType = PostType.Post // Default type if no media
            };

            _unitOfWork.Repository<Post>().Add(post);
            int postSaveResult = await _unitOfWork.CompleteAsync();

            if (postSaveResult <= 0)
                throw new Exception("An error occurred while saving the post.");

            // If there are no media files, return early
            if (dto.MediaFiles == null || !dto.MediaFiles.Any())
            {
                return new PostToReturnDto
                {
                    PostId = post.Id,
                    Caption = post.Caption,
                    FilesUrls = new List<string>(),
                    PostType = post.PostType.ToString()
                };
            }

            // Save media files
            var mediaItems = new List<Media>();
            var mediaUrls = new List<string>();
            int videoCount = 0;

            foreach (var file in dto.MediaFiles)
            {
                if (file.Length > 0)
                {
                    var fileUrl = await _fileUploadService.UploadFileAsync(file, "posts");
                    var mediaType = Helper.GetMediaType(file.ContentType);

                    if (mediaType == MediaType.Video)
                        videoCount++;

                    var media = new Media
                    {
                        Url = fileUrl,
                        Type = mediaType,
                        CreatedAt = DateTime.UtcNow,
                        PostId = post.Id
                    };

                    mediaItems.Add(media);
                    mediaUrls.Add(fileUrl);
                }
            }

            await _unitOfWork.Repository<Media>().AddRange(mediaItems);
            int mediaSaveResult = await _unitOfWork.CompleteAsync();

            if (mediaSaveResult <= 0)
                throw new Exception("An error occurred while saving media files.");

            // Determine PostType
            post.PostType = (videoCount == 1 && mediaItems.Count == 1) ? PostType.Video : PostType.Post;

            if (dto.Type != null && dto.Type == "Clip" && videoCount == 1 && mediaItems.Count == 1)
                post.PostType = PostType.Clip;

            _unitOfWork.Repository<Post>().Update(post);
            await _unitOfWork.CompleteAsync();

            return new PostToReturnDto
            {
                PostId = post.Id,
                Caption = post.Caption,
                FilesUrls = mediaUrls,
                PostType = post.PostType.ToString()
            };

        }

        //public async Task<bool> DeletePostAsync(int postId)
        //{
        //    if (postId <= 0)
        //        throw new Exception($"Invalid {postId}");

        //    var spec = new PostWithMediaSpecification(postId);

        //    var post = await _unitOfWork.Repository<Post>().GetByIdWithSpecAsync(spec);

        //    if (post == null)
        //        throw new Exception("Post not founded");

        //    // Delete media files

        //    if (post.MediaItems != null && post.MediaItems.Any())
        //    {
        //        foreach (var media in post.MediaItems)
        //        {
        //            if (!string.IsNullOrEmpty(media.Url))
        //            {
        //                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "posts", media.Url);

        //                filePath = $"wwwroot{filePath}";

        //                if (File.Exists(filePath))
        //                    File.Delete(filePath);
        //            }
        //        }
        //    }


        //    _unitOfWork.Repository<Post>().Delete(post);

        //    int result = await _unitOfWork.CompleteAsync();

        //    if (result <= 0)
        //        throw new Exception("An error occurred while Deleting post.");


        //    return true;
        //}

        public async Task<bool> DeletePostAsync(int postId)
        {
            if (postId <= 0)
                throw new Exception($"Invalid {postId}");

            var spec = new PostWithMediaSpecification(postId);
            var post = await _unitOfWork.Repository<Post>().GetByIdWithSpecAsync(spec);

            if (post == null)
                throw new Exception("Post not founded");

            // Delete related reactions

            var reactionSpec = new ReactionsByPostSpecification(postId);

            var reactions = await _unitOfWork.Repository<Reaction>().GetAllWithSpecAsync(reactionSpec);
            foreach (var reaction in reactions)
            {
                _unitOfWork.Repository<Reaction>().Delete(reaction);
            }

            // Delete related comments (if needed)

            var commentSpec = new CommentsForPostSpecification(postId);

            var comments = await _unitOfWork.Repository<Comment>().GetAllWithSpecAsync(commentSpec);
            foreach (var comment in comments)
            {
                _unitOfWork.Repository<Comment>().Delete(comment);
            }

            // Delete related media files
            if (post.MediaItems != null && post.MediaItems.Any())
            {
                foreach (var media in post.MediaItems)
                {
                    if (!string.IsNullOrEmpty(media.Url))
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "posts", media.Url);
                        filePath = $"wwwroot{filePath}";
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                    _unitOfWork.Repository<Media>().Delete(media);
                }
            }

            _unitOfWork.Repository<Post>().Delete(post);

            int result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                throw new Exception("An error occurred while Deleting post.");

            return true;
        }

        public async Task<IReadOnlyList<FeedDto>> GetAllPostsAsync(int userId, string? feel = null)
        {
            ///var spec = new GetPostsSpecification(feel);
            ///
            ///var posts = await _unitOfWork.Repository<Post>().GetAllWithSpecAsync(spec);
            ///
            ///if (posts == null || !posts.Any())
            ///    throw new Exception("No Posts founded");
            ///
            ///return posts.Select(post => new FeedDto
            ///{
            ///    PostId = post.Id,
            ///    Caption = post.Caption,
            ///    FilesUrls = post.MediaItems?.Select(m => m.Url).ToList() ?? new List<string>(),
            ///
            ///    UserId = post.User.Id,
            ///    UserName = $"{post.User.FirstName} {post.User.LastName}",
            ///    UserImage = post.User.ProfilePic,
            ///
            ///    ReactionsCount = post.Reactions?.Count ?? 0,
            ///    CommentsCount = post.Comments?.Count ?? 0,
            ///    Date = Helper.FormatDate(post.CreatedAt),
            ///
            ///    SharesCount = _unitOfWork.Repository<SharedPost>().CountAsync(sp => sp.PostId == sp.Post.Id).Result
            ///
            ///}).ToList().AsReadOnly();
            ///

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);

            if (user == null)
                throw new Exception("User not founded!");

            if (feel is not null && feel.Equals("sad", StringComparison.CurrentCultureIgnoreCase))
                user.IsSad = true;
            else
                user.IsSad = false;

            var spec = new GetPostsSpecification(feel);

            var posts = await _unitOfWork.Repository<Post>().GetAllWithSpecAsync(spec);

            var sharedPostsSpec = new SharedPostsSpecification();

            var sharedPosts = await _unitOfWork.Repository<SharedPost>().GetAllWithSpecAsync(sharedPostsSpec);

            var allPosts = posts.Select(post => new FeedDto
            {
                PostId = post.Id,
                Caption = post.Caption,
                FilesUrls = post.MediaItems?.Select(m => m.Url).ToList() ?? new List<string>(),
                UserId = post.User.Id,
                UserName = $"{post.User.FirstName} {post.User.LastName}",
                UserImage = post.User.ProfilePic,
                ReactionsCount = post.Reactions?.Count ?? 0,
                CommentsCount = post.Comments?.Count ?? 0,
                SharesCount = _unitOfWork.Repository<SharedPost>().CountAsync(sp => sp.PostId == post.Id).Result,
                Date = Helper.FormatDate(post.CreatedAt),
                IsShared = false,
                PostType = post.PostType.ToString()

            }).ToList();

            allPosts.AddRange(sharedPosts.Select(sp => new FeedDto
            {
                PostId = sp.Post.Id,
                Caption = sp.Post.Caption,
                FilesUrls = sp.Post.MediaItems?.Select(m => m.Url).ToList() ?? new List<string>(),
                UserId = sp.Post.User.Id,
                UserName = $"{sp.Post.User.FirstName} {sp.Post.User.LastName}",
                UserImage = sp.Post.User.ProfilePic,
                ReactionsCount = sp.Post.Reactions?.Count ?? 0,
                CommentsCount = sp.Post.Comments?.Count ?? 0,
                SharesCount = _unitOfWork.Repository<SharedPost>().CountAsync(sp => sp.PostId == sp.Post.Id).Result,
                Date = Helper.FormatDate(sp.Post.CreatedAt),
                IsShared = true,
                PostType = sp.Post.PostType.ToString()

            }));

            _unitOfWork.Repository<User>().Update(user);
            await _unitOfWork.CompleteAsync();

            return allPosts.OrderByDescending(p => p.PostId).ToList().AsReadOnly();
        }
        public async Task<bool> ReportPostAsync(ReportPostDto dto)
        {
            var post = await _unitOfWork.Repository<Post>().GetByIdAsync(dto.PostId);
            if (post == null)
                throw new Exception("Post not found.");

            var reporter = await _unitOfWork.Repository<User>().GetByIdAsync(dto.ReporterId);


            if (reporter == null)
                throw new Exception("Reporter not found.");

            var reportIsExists = await _unitOfWork.Repository<ReportPost>()
                .FirstOrDefaultAsync(rp => rp.PostId == dto.PostId && rp.ReporterId == dto.ReporterId);

            if (reportIsExists != null)
                {
                throw new Exception("You have already reported this post.");
            }

            var report = new ReportPost
            {
                PostId = dto.PostId,
                ReporterId = dto.ReporterId,
                //Reason = dto.Reason,
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.Repository<ReportPost>().Add(report);
            int result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                throw new Exception("An error occurred while reporting the post.");

            // Optionally: Notify admins or log the report here

            return true;
        }

        // add method to return reported posts Ids and count of reports on each post return dto with PostId and Count of reports

        public async Task<IReadOnlyList<ReportPostToReturnDto>> GetReportedPosts()
        {
            var reportedPosts = await _unitOfWork.Repository<ReportPost>().GetAllAsync();

            var result = reportedPosts
                .GroupBy(post => post.PostId)
                .Select(group => new ReportPostToReturnDto
                {
                    PostId = group.Key,
                    ReportsCount = group.Count()
                })
                .ToList();

            return result;
        }

        public async Task<bool> RemoveReportedPost(int postId)
        {
            try
            {
                var spec = new ReportedPostsSpecification(postId);

                var reportsToRemove = await _unitOfWork.Repository<ReportPost>()
                    .GetAllWithSpecAsync(spec);

                if (!reportsToRemove.Any())
                {
                   throw new Exception("No reports found for this post.");
                }

                // Remove all reports for this post
                foreach (var report in reportsToRemove)
                {
                    _unitOfWork.Repository<ReportPost>().Delete(report);
                }

                // Save changes
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception)
            {
                // Log exception if needed
                return false;
            }
        }
        public async Task<IReadOnlyList<PostToReturnDto>> GetPostsByUserAsync(int userId)
        {

            ///if (userId <= 0)
            ///    throw new Exception($"Invalid UserId {userId}");
            ///
            ///var spec = new PostsByUserSpecification(userId);
            ///
            ///var posts = await _unitOfWork.Repository<Post>().GetAllWithSpecAsync(spec);
            ///
            ///if (posts == null || !posts.Any())
            ///    throw new Exception("No Posts founded");
            ///
            ///return posts.Select(post => new PostToReturnDto
            ///{
            ///    PostId = post.Id,
            ///    Caption = post.Caption,
            ///    FilesUrls = post.MediaItems?.Select(m => m.Url).ToList() ?? new List<string>(),
            ///    UserId = post.User.Id,
            ///    UserName = $"{post.User.FirstName} {post.User.LastName}",
            ///    UserImage = post.User.ProfilePic,
            ///    ReactionsCount = post.Reactions?.Count ?? 0,
            ///    CommentsCount = post.Comments?.Count ?? 0,
            ///    Date = Helper.FormatDate(post.CreatedAt)
            ///
            ///}).ToList().AsReadOnly();

            var spec = new PostsByUserSpecification(userId);

            var posts = await _unitOfWork.Repository<Post>().GetAllWithSpecAsync(spec);

            var sharedPostsSpec = new SharedPostsByUserSpecification(userId);

            var sharedPosts = await _unitOfWork.Repository<SharedPost>().GetAllWithSpecAsync(sharedPostsSpec);

            var allPosts = posts.Select(post => new PostToReturnDto
            {
                PostId = post.Id,
                Caption = post.Caption,
                FilesUrls = post.MediaItems?.Select(m => m.Url).ToList() ?? new List<string>(),
                UserId = post.User.Id,
                UserName = $"{post.User.FirstName} {post.User.LastName}",
                UserImage = post.User.ProfilePic,
                ReactionsCount = post.Reactions?.Count ?? 0,
                CommentsCount = post.Comments?.Count ?? 0,
                SharesCount = _unitOfWork.Repository<SharedPost>().CountAsync(sp => sp.PostId == post.Id).Result,
                Date = Helper.FormatDate(post.CreatedAt),
                IsShared = false,
                PostType = post.PostType.ToString()

            }).ToList();

            allPosts.AddRange(sharedPosts.Select(sp => new PostToReturnDto
            {
                PostId = sp.Post.Id,
                Caption = sp.Post.Caption,
                FilesUrls = sp.Post.MediaItems?.Select(m => m.Url).ToList() ?? new List<string>(),
                UserId = sp.Post.User.Id,
                UserName = $"{sp.Post.User.FirstName} {sp.Post.User.LastName}",
                UserImage = sp.Post.User.ProfilePic,
                ReactionsCount = sp.Post.Reactions?.Count ?? 0,
                CommentsCount = sp.Post.Comments?.Count ?? 0,
                SharesCount = _unitOfWork.Repository<SharedPost>().CountAsync(sp => sp.PostId == sp.Post.Id).Result,
                Date = Helper.FormatDate(sp.Post.CreatedAt),
                IsShared = true,
                PostType = sp.Post.PostType.ToString()

            }));

            return allPosts.OrderByDescending(p => p.PostId).ToList().AsReadOnly();

        }

        public async Task<bool> AddFavoritePostAsync(int userId, int postId)
        {
            var existingFavorite = await _unitOfWork.Repository<FavoritePost>()
                .FirstOrDefaultAsync(fp => fp.UserId == userId && fp.PostId == postId);

            if (existingFavorite != null)
            {
                throw new Exception("This post is already in your favorites.");
            }

            var favoritePost = new FavoritePost
            {
                UserId = userId,
                PostId = postId
            };

            _unitOfWork.Repository<FavoritePost>().Add(favoritePost);
            int result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
            {
                throw new Exception("An error occurred while adding the post to favorites.");
            }

            return true;
        }

        public async Task<IReadOnlyList<PostToReturnDto>> GetFavoritePostsAsync(int userId)
        {
            var spec = new FavoritePostsSpecification(userId);

            var favoritePosts = await _unitOfWork.Repository<FavoritePost>().GetAllWithSpecAsync(spec);

            if (favoritePosts == null || !favoritePosts.Any())
            {
                throw new Exception("No favorite posts found.");
            }

            return favoritePosts.Select(fp => new PostToReturnDto
            {
                PostId = fp.Post.Id,
                Caption = fp.Post.Caption,
                CommentsCount = fp.Post.Comments?.Count ?? 0,
                ReactionsCount = fp.Post.Reactions?.Count ?? 0,
                SharesCount = _unitOfWork.Repository<SharedPost>().CountAsync(sp => sp.PostId == fp.Post.Id).Result,
                Date = Helper.FormatDate(fp.Post.CreatedAt),
                UserId = fp.Post.User.Id,
                IsShared = false,
                UserName = $"{fp.Post.User.FirstName} {fp.Post.User.LastName}",
                UserImage = fp.Post.User.ProfilePic,
                FilesUrls = fp.Post.MediaItems?.Select(m => m.Url).ToList() ?? new List<string>(),
                PostType = fp.Post.PostType.ToString()


            }).ToList().AsReadOnly();
        }

        public async Task<bool> RemoveFavoritePostAsync(int userId, int postId)
        {
            var favoritePost = await _unitOfWork.Repository<FavoritePost>()
            .FirstOrDefaultAsync(fp => fp.UserId == userId && fp.PostId == postId);

            if (favoritePost == null)
            {
                throw new Exception("Favorite post not found.");
            }

            _unitOfWork.Repository<FavoritePost>().Delete(favoritePost);
            int result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
            {
                throw new Exception("An error occurred while removing the post from favorites.");
            }

            return true;
        }

        public async Task<bool> SharePostAsync(int userId, int postId)
        {

            var post = await _unitOfWork.Repository<Post>().GetByIdAsync(postId);

            if (post == null)
            {
                throw new Exception("Post Not founded!");
            }

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);

            if (user == null)
                throw new Exception($"User Not founded with this Id {userId}");

            var sharedPost = new SharedPost
            {
                UserId = userId,
                PostId = postId
            };

            _unitOfWork.Repository<SharedPost>().Add(sharedPost);

            int result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
            {
                throw new Exception("An error occurred while sharing the post.");
            }

            await _notificationService.CreateShareNotificationAsync(userId, post.UserId, postId);

            await _activityLogService.LogActivityAsync(
               userId,
               ActivityType.Share,
               postId
             );

            return true;

        }


        public async Task<IReadOnlyList<PostToReturnDto>> GetSharedPostsAsync(int userId)
        {
            var spec = new SharedPostsSpecification(userId);

            var sharedPosts = await _unitOfWork.Repository<SharedPost>().GetAllWithSpecAsync(spec);

            if (sharedPosts == null || !sharedPosts.Any())
                throw new Exception("No shared posts found.");

            return sharedPosts.Select(sp => new PostToReturnDto
            {
                PostId = sp.Post.Id,
                Caption = sp.Post.Caption,
                FilesUrls = sp.Post.MediaItems?.Select(m => m.Url).ToList() ?? new List<string>(),
                UserId = sp.Post.User.Id,
                UserName = $"{sp.Post.User.FirstName} {sp.Post.User.LastName}",
                UserImage = sp.Post.User.ProfilePic,
                ReactionsCount = sp.Post.Reactions?.Count ?? 0,
                CommentsCount = sp.Post.Comments?.Count ?? 0,
                Date = Helper.FormatDate(sp.Post.CreatedAt)

            }).ToList().AsReadOnly();
        }

        public async Task<bool> IsFavoritePostAsync(int postId, int userId)
        {
            var favoritePost = await _unitOfWork.Repository<FavoritePost>()
            .FirstOrDefaultAsync(fp => fp.UserId == userId && fp.PostId == postId);

            if (favoritePost == null)
                return false;

            return true;

        }
    }

}

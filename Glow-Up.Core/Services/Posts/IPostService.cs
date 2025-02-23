using Glow_Up.Core.DTOs.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Services.Posts
{
    public interface IPostService
    {
        Task<PostToReturnDto> CreateNewPostAsync(int userId, CreatePostDto dto);
        Task<bool> DeletePostAsync(int postId);

        Task<IReadOnlyList<FeedDto>> GetAllPostsAsync(string? feel = null);
        Task<bool> AddReactionAsync(int postId, AddReactionDto dto);

        Task<IReadOnlyList<PostToReturnDto>> GetPostsByUserAsync(int userId);

        // Fav posts 

        Task<bool> AddFavoritePostAsync(int userId, int postId);

        Task<bool> RemoveFavoritePostAsync(int userId, int postId);
        Task<IReadOnlyList<PostToReturnDto>> GetFavoritePostsAsync(int userId);

        Task<bool> SharePostAsync(int userId, int postId);
        Task<IReadOnlyList<PostToReturnDto>> GetSharedPostsAsync(int userId);

        Task<bool> IsFavoritePostAsync(int postId, int userId);
    }
}

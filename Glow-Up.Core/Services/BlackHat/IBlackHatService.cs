using Glow_Up.Core.DTOs.BHComment;
using Glow_Up.Core.DTOs.BHPost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Services.BlackHat
{
    public interface IBlackHatService
    {
        Task<BHPostToReturnDto> CreatePostAsync(int userId, CreateBHPostDto dto, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<BHPostToReturnDto>> GetAllPostsAsync(string? category, CancellationToken cancellationToken = default);

        Task<BHCommentToReturnDto> AddCommentAsync(int postId, CreateBHCommentDto dto, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<BHCommentToReturnDto>> GetAllCommentsAsync(int postId, CancellationToken cancellationToken = default);

        Task<bool> IncreaseCommentAsync(int commentId, int userId, CancellationToken cancellationToken = default);

        Task<bool> DecreaseCommentAsync(int commentId, int userId, CancellationToken cancellationToken = default);

        Task<bool> LikePostAsync(int postId, int userId, CancellationToken cancellationToken = default);

        Task<bool> UnLikePostAsync(int postId, int userId, CancellationToken cancellationToken = default);

        Task<bool> HasUserLikedPostAsync(int postId, int userId, CancellationToken cancellationToken = default);

        Task<BHPostToReturnDto> GetMostPopularPostAsync(CancellationToken cancellationToken = default);
    }
}

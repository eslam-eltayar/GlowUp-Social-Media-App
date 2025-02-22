using Glow_Up.Core.DTOs.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Services.Comment
{
    public interface ICommentService
    {
        Task<CommentToReturnDto> CreateCommentAsync(int userId, int postId, AddCommentDto dto);
        Task<bool> DeleteCommentAsync(int commentId);
        Task<IReadOnlyList<CommentToReturnDto>> GetCommentsForPostAsync(int postId);

        Task<CommentToReturnDto> ReplyToCommentAsync(int userId, int postId, CreateReplyDto dto);

    }
}

using Glow_Up.Core.DTOs.Comment;
using Glow_Up.Core.Services.Comment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Glow_Up.APIs.Controllers
{
    public class CommentsController : ApiBaseController
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("AddCommentOnPost/{userId:int}/{postId:int}")]
        public async Task<ActionResult<CommentToReturnDto>> AddCommentOnPost(int userId, int postId, [FromForm] AddCommentDto dto)
        {
            try
            {
                var comment = await _commentService.CreateCommentAsync(userId, postId, dto);

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("GetCommentsForPost/{postId:int}")]
        public async Task<ActionResult<IReadOnlyList<CommentToReturnDto>>> GetCommentsForPost(int postId)
        {
            try
            {
                var comments = await _commentService.GetCommentsForPostAsync(postId);

                if (comments == null || !comments.Any())
                {
                    return NotFound(new { Message = "No comments found for this post." });
                }

                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.InnerException });
            }
        }

        [HttpDelete("DeleteComment/{commentId:int}")]
        public async Task<ActionResult> DeleteComment(int commentId)
        {
            try
            {
                var result = await _commentService.DeleteCommentAsync(commentId);

                if (result)
                {
                    return Ok(new { Message = "Comment deleted successfully." });
                }
                else
                {
                    return NotFound(new { Message = "Comment not found." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPost("ReplyToComment/{userId:int}/{postId:int}")]
        public async Task<ActionResult<CommentToReturnDto>> ReplyToComment(int userId, int postId, [FromForm] CreateReplyDto dto)
        {
            try
            {
                var reply = await _commentService.ReplyToCommentAsync(userId, postId, dto);

                return Ok(reply);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }



    }
}

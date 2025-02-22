using Glow_Up.Core.DTOs.BHComment;
using Glow_Up.Core.DTOs.BHPost;
using Glow_Up.Core.DTOs.Post;
using Glow_Up.Core.Services.BlackHat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Glow_Up.APIs.Controllers
{
    public class BlackHatController(IBlackHatService blackHatService) : ApiBaseController
    {
        private readonly IBlackHatService _blackHatService = blackHatService;

        [HttpPost("AddPost/{userId}")]
        public async Task<ActionResult<BHPostToReturnDto>> CreatePost(int userId, [FromForm] CreateBHPostDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var post = await _blackHatService.CreatePostAsync(userId, dto, cancellationToken);

                return Ok(post);
            }
            catch (Exception ex)
            {

                return BadRequest(new { Message = ex.Message });

            }
        }

        [HttpGet("GetAllPosts")]
        public async Task<ActionResult<IReadOnlyList<BHPostToReturnDto>>> GetAllPosts(CancellationToken cancellationToken, [FromQuery] string? category = null)
        {
            try
            {
                var posts = await _blackHatService.GetAllPostsAsync(category, cancellationToken);

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost("AddComment/{postId}")]
        public async Task<ActionResult<BHCommentToReturnDto>> AddComment(int postId, [FromBody] CreateBHCommentDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var comment = await _blackHatService.AddCommentAsync(postId, dto, cancellationToken);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("AllComments/{postId}")]
        public async Task<ActionResult<IReadOnlyList<BHCommentToReturnDto>>> GetAllComments(int postId, CancellationToken cancellationToken)
        {
            try
            {
                var comments = await _blackHatService.GetAllCommentsAsync(postId, cancellationToken);

                return Ok(comments);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost("IncreaseComment/{commentId}")]
        public async Task<IActionResult> IncreaseComment(int commentId, [FromQuery] int userId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _blackHatService.IncreaseCommentAsync(commentId, userId, cancellationToken);

                return Ok(new { Message = "Comment Increased Successfully." });
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost("DecreaseComment/{commentId}")]
        public async Task<IActionResult> DecreaseComment(int commentId, [FromQuery] int userId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _blackHatService.DecreaseCommentAsync(commentId, userId, cancellationToken);

                return Ok(new { Message = "Comment Decreased Successfully." });
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost("LikePost/{postId}")]
        public async Task<IActionResult> LikePost(int postId, [FromQuery] int userId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _blackHatService.LikePostAsync(postId, userId, cancellationToken);

                return Ok(new { Message = "Post Liked Successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("UnLikePost/{postId}")]
        public async Task<IActionResult> UnLikePost(int postId, [FromQuery] int userId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _blackHatService.UnLikePostAsync(postId, userId, cancellationToken);

                return Ok(new { Message = "Post UnLiked Successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("HasLiked/{postId}")]
        public async Task<IActionResult> HasUserLikedPost(int postId, [FromQuery] int userId, CancellationToken cancellationToken)
        {
            try
            {
                bool hasLiked = await _blackHatService.HasUserLikedPostAsync(postId, userId, cancellationToken);
                return Ok(new { hasLiked });
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("MostPopularPostThisDay")]
        public async Task<ActionResult<BHPostToReturnDto>> GetMostPopularPostThisDay(CancellationToken cancellationToken)
        {
            try
            {
                var post = await _blackHatService.GetMostPopularPostAsync(cancellationToken);
                return Ok(post);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}

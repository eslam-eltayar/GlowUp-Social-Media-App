using Glow_Up.Core.DTOs.Post;
using Glow_Up.Core.Models;
using Glow_Up.Core.Services.Posts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Glow_Up.APIs.Controllers
{
    public class PostsController : ApiBaseController
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("AddNewPost/{userId}")]
        public async Task<ActionResult<PostToReturnDto>> AddNewPost(int userId, [FromForm] CreatePostDto dto)
        {
            try
            {
                //var userId = GetCurrentUserId();

                var post = await _postService.CreateNewPostAsync(userId, dto);

                return post != null ?
                     Ok(post) : BadRequest(new { Message = $"An Error while creating post" }); ;
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"{ex.Message}" });
            }
        }

        [HttpDelete("DeletePost/{postId}")]
        public async Task<ActionResult<PostToReturnDto>> DeletePost(int postId)
        {
            try
            {
                var result = await _postService.DeletePostAsync(postId);

                return Ok(new { message = "Post deleted successfully." });

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"{ex.Message}" });
            }
        }



        [HttpGet("GetAllPosts")]
        public async Task<ActionResult<IReadOnlyList<FeedDto>>> GetAllPosts([FromQuery] string? feel = null)
        {
            try
            {
                var posts = await _postService.GetAllPostsAsync(feel);

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost("AddReactToPost/{postId}")]
        public async Task<IActionResult> AddReaction(int postId, [FromBody] AddReactionDto dto)
        {
            try
            {
                var result = await _postService.AddReactionAsync(postId, dto);

                return Ok(new { Message = "React added successfully." });
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("PostsByUser/{userId}")]
        public async Task<ActionResult<IReadOnlyList<PostToReturnDto>>> GetPostsByUser(int userId)
        {
            try
            {
                var posts = await _postService.GetPostsByUserAsync(userId);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // Fav Posts endpoints


        [HttpPost("AddFavoritePost/{userId}/{postId}")]
        public async Task<IActionResult> AddFavoritePost(int userId, int postId)
        {
            try
            {
                var result = await _postService.AddFavoritePostAsync(userId, postId);

                if (result)
                    return Ok(new { Message = "Post added to favorites successfully." });
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"{ex.Message}" });
            }
        }

        [HttpDelete("FavoritePost/{userId}/{postId}")]
        public async Task<IActionResult> RemoveFavoritePost(int userId, int postId)
        {
            try
            {
                var result = await _postService.RemoveFavoritePostAsync(userId, postId);

                if (result)
                    return Ok(new { Message = "Post removed from favorites successfully." });
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"{ex.Message}" });
            }
        }

        [HttpGet("FavoritePosts/{userId}")]
        public async Task<ActionResult<IReadOnlyList<PostToReturnDto>>> GetFavoritePosts(int userId)
        {
            try
            {
                var favoritePosts = await _postService.GetFavoritePostsAsync(userId);
                return Ok(favoritePosts);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost("SharePost/{userId}/{postId}")]
        public async Task<IActionResult> SharePost(int userId, int postId)
        {
            try
            {
                var result = await _postService.SharePostAsync(userId, postId);

                if (result)
                    return Ok(new { Message = "Post shared successfully." });
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("SharedPosts/{userId}")]
        public async Task<ActionResult<IReadOnlyList<PostToReturnDto>>> GetSharedPosts(int userId)
        {
            try
            {
                var sharedPosts = await _postService.GetSharedPostsAsync(userId);
                return Ok(sharedPosts);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        //[NonAction]
        //private int GetCurrentUserId()
        //{
        //    var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
        //    {
        //        throw new UnauthorizedAccessException("User ID not found in claims.");
        //    }

        //    return userId;
        //}

    }
}

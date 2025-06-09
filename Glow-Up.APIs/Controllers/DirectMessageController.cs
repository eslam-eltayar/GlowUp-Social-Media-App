using Glow_Up.Core.DTOs.Messages;
using Glow_Up.Core.Services.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Glow_Up.APIs.Controllers;

public class DirectMessageController : ApiBaseController
{
    private readonly IMessageService _messageService;

    public DirectMessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost("send/{recipientId}")]
    public async Task<ActionResult<MessageDto>> SendMessage(
        int recipientId,
        [FromBody] CreateMessageDto dto,
        [FromQuery] int senderId)
    {
        try
        {
            var message = await _messageService.SendMessageAsync(senderId, recipientId, dto.Content);
            return Ok(message);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("messages/{otherUserId}")]
    public async Task<ActionResult<IReadOnlyList<MessageDto>>> GetMessages(
        int otherUserId,
        [FromQuery] int userId)
    {
        try
        {
            var messages = await _messageService.GetUserMessagesAsync(userId, otherUserId);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("unread")]
    public async Task<ActionResult<IReadOnlyList<MessageDto>>> GetUnreadMessages([FromQuery] int userId)
    {
        try
        {
            var messages = await _messageService.GetUnreadMessagesAsync(userId);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("chats")]
    public async Task<ActionResult<IReadOnlyList<ChatDto>>> GetUserChats([FromQuery] int userId)
    {
        try
        {
            var chats = await _messageService.GetUserChatsAsync(userId);
            return Ok(chats);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPut("mark-as-read/{senderId}")]
    public async Task<IActionResult> MarkMessagesAsRead(int senderId, [FromQuery] int userId)
    {
        try
        {
            await _messageService.MarkMessagesAsReadAsync(userId, senderId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}

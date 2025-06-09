using Glow_Up.Core.DTOs.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Services.Messages;
public interface IMessageService
{
    Task<MessageDto> SendMessageAsync(int senderId, int recipientId, string content);
    Task<IReadOnlyList<MessageDto>> GetUserMessagesAsync(int userId, int otherUserId);
    Task<IReadOnlyList<MessageDto>> GetUnreadMessagesAsync(int userId);
    Task<bool> MarkMessagesAsReadAsync(int userId, int senderId);
    Task<IReadOnlyList<ChatDto>> GetUserChatsAsync(int userId);
}

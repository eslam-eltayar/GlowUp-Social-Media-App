using Glow_Up.Core.DTOs.Messages;
using Glow_Up.Core.Models;
using Glow_Up.Core.Repositories;
using Glow_Up.Core.Services.Messages;
using Glow_Up.Core.Services.Notifications;
using Glow_Up.Core.Specifications.Messages_Spec;
using Glow_Up.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Services.Messages;
// Glow_Up.Services\Messages\MessageService.cs
public class MessageService : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessagePublisher _messagePublisher;
    private readonly INotificationService _notificationService;

    public MessageService(
        IUnitOfWork unitOfWork,
        IMessagePublisher messagePublisher,
        INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _messagePublisher = messagePublisher;
        _notificationService = notificationService;
    }

    public async Task<MessageDto> SendMessageAsync(int senderId, int recipientId, string content)
    {
        var message = new Message
        {
            SenderId = senderId,
            RecipientId = recipientId,
            Content = content,
            SentAt = DateTime.UtcNow,
            IsRead = false
        };

        _unitOfWork.Repository<Message>().Add(message);
        await _unitOfWork.CompleteAsync();

        var messageDto = await MapMessageToDto(message);

        // Publish the message via SignalR
        await _messagePublisher.PublishMessageAsync(messageDto);

        // Create a notification for the message
        await _notificationService.CreateMessageNotificationAsync(senderId, recipientId, message.Id);

        return messageDto;
    }

    public async Task<IReadOnlyList<MessageDto>> GetUserMessagesAsync(int userId, int otherUserId)
    {
        var spec = new UserMessagesSpecification(userId, otherUserId);

        var messages = await _unitOfWork.Repository<Message>()
            .GetAllWithSpecAsync(spec);

        var messageDtos = new List<MessageDto>();

        foreach (var message in messages.OrderBy(m => m.SentAt))
        {
            messageDtos.Add(await MapMessageToDto(message));
        }

        return messageDtos;
    }

    public async Task<IReadOnlyList<MessageDto>> GetUnreadMessagesAsync(int userId)
    {
        var spec = new UserMessagesSpecification(userId);

        var messages = await _unitOfWork.Repository<Message>()
            .GetAllWithSpecAsync(spec);

        var messageDtos = new List<MessageDto>();

        foreach (var message in messages.OrderBy(m => m.SentAt))
        {
            messageDtos.Add(await MapMessageToDto(message));
        }

        return messageDtos;
    }

    public async Task<bool> MarkMessagesAsReadAsync(int userId, int senderId)
    {
        var spec = new UserUnReadMessagesSpecification(userId, senderId);

        var messages = await _unitOfWork.Repository<Message>()
            .GetAllWithSpecAsync(spec);

        foreach (var message in messages)
        {
            message.IsRead = true;
            _unitOfWork.Repository<Message>().Update(message);
        }

        var result = await _unitOfWork.CompleteAsync();
        return result > 0;
    }

    public async Task<IReadOnlyList<ChatDto>> GetUserChatsAsync(int userId)
    {
        var spec = new UserChatsSpecification(userId);
        var allMessages = await _unitOfWork.Repository<Message>().GetAllWithSpecAsync(spec);

        var chats = allMessages
            .GroupBy(m => m.SenderId == userId ? m.RecipientId : m.SenderId)
            .Select(async group =>
            {
                var otherUserId = group.Key;
                var lastMessage = group.OrderByDescending(m => m.SentAt).First();
                var unreadCount = group.Count(m => !m.IsRead && m.RecipientId == userId);

                var otherUser = await _unitOfWork.Repository<User>().GetByIdAsync(otherUserId);

                return new ChatDto
                {
                    UserId = otherUserId,
                    UserName = $"{otherUser.FirstName} {otherUser.LastName}",
                    ProfilePic = otherUser.ProfilePic,
                    LastMessage = await MapMessageToDto(lastMessage),
                    UnreadCount = unreadCount
                };
            })
            .ToList();

        var results = await Task.WhenAll(chats);

        return results
            .OrderByDescending(c => c.LastMessage.SentAt)
            .ToList()
            .AsReadOnly();
    }

    private async Task<MessageDto> MapMessageToDto(Message message)
    {
        var sender = await _unitOfWork.Repository<User>().GetByIdAsync(message.SenderId);
        var recipient = await _unitOfWork.Repository<User>().GetByIdAsync(message.RecipientId);

        return new MessageDto
        {
            Id = message.Id,
            SenderId = message.SenderId,
            SenderName = $"{sender.FirstName} {sender.LastName}",
            SenderProfilePic = sender.ProfilePic,
            RecipientId = message.RecipientId,
            RecipientName = $"{recipient.FirstName} {recipient.LastName}",
            RecipientProfilePic = recipient.ProfilePic,
            Content = message.Content,
            SentAt = Helper.FormatDate(message.SentAt),
            IsRead = message.IsRead
        };
    }

}
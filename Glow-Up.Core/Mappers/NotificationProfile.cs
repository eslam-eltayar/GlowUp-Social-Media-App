using AutoMapper;
using Glow_Up.Core.DTOs.Notifications;
using Glow_Up.Core.Models;

namespace Glow_Up.Core.Mappers
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>();
        }
    }
}

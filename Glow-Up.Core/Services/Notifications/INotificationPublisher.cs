using Glow_Up.Core.DTOs.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Services.Notifications
{
    public interface INotificationPublisher
    {
        Task PublishNotificationAsync(NotificationDto notification);
    }
}

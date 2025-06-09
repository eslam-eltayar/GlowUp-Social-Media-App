using Glow_Up.Core.DTOs.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Services.Messages;
public interface IMessagePublisher
{
    Task PublishMessageAsync(MessageDto message);
}


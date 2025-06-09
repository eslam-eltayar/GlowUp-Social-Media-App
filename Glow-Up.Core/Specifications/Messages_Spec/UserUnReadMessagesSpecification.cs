using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.Messages_Spec;
public class UserUnReadMessagesSpecification : BaseSpecification<Message>
{
    public UserUnReadMessagesSpecification(int userId, int senderId)
        :base(
            m => m.RecipientId == userId && m.SenderId == senderId && !m.IsRead
        )
    {
        
    }
}

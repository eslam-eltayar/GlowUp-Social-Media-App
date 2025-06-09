using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.Messages_Spec;
public class UserMessagesSpecification : BaseSpecification<Message>
{
    public UserMessagesSpecification(int userId, int otherUserId) :
        base(m =>
            (m.SenderId == userId && m.RecipientId == otherUserId) ||
            (m.SenderId == otherUserId && m.RecipientId == userId)
        )
    {

    }

    public UserMessagesSpecification(int userId)
        :base(
            m => m.RecipientId == userId && !m.IsRead
        )
    {
        
    }
}

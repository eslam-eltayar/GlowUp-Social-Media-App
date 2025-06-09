using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.Messages_Spec;
public class UserChatsSpecification : BaseSpecification<Message>
{
    public UserChatsSpecification(int userId)
        : base(m => m.SenderId == userId || m.RecipientId == userId)
    {
        AddIncludes();
        ApplyOrderByDescending(m => m.SentAt);
    }

    private void AddIncludes()
    {
        Includes.Add(m => m.Sender);
        Includes.Add(m => m.Recipient);
    }
}

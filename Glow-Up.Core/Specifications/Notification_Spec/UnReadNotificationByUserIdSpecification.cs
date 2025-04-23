using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.Notification_Spec
{
    public class UnReadNotificationByUserIdSpecification : BaseSpecification<Notification>
    {
        public UnReadNotificationByUserIdSpecification(int userId)
            : base(x => x.RecipientId == userId && !x.IsRead)
        {
            
        }
    }
}

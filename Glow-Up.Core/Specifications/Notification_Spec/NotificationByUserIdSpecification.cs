using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.Notification_Spec
{
    public class NotificationByUserIdSpecification : BaseSpecification<Notification>
    {
        public NotificationByUserIdSpecification(int userId)
            : base(x => x.RecipientId == userId)
        {
            AddIncludes();
            ApplyOrderByDescending(x => x.Id);
        }

        private void AddIncludes()
        {
            Includes.Add(x => x.Sender);
            Includes.Add(x => x.Recipient);
        }
    }
}

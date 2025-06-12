using Glow_Up.Core.Enums;
using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.LogSpec;
public class UserBHActivitiesSpecification : BaseSpecification<ActivityLog>
{
    public UserBHActivitiesSpecification(int userId)
        : base(log =>
            log.UserId == userId &&
            (log.Type == ActivityType.CreateBHPost ||
             log.Type == ActivityType.BHComment ||
             log.Type == ActivityType.BHVote ||
             log.Type == ActivityType.BHLike ||
             log.Type == ActivityType.BHUnlike))
    {
        AddIncludes();
        ApplyOrderByDescending(log => log.CreatedAt);
    }

    public UserBHActivitiesSpecification(int userId, DateTime fromDate, DateTime toDate)
        : base(log =>
            log.UserId == userId &&
            log.CreatedAt >= fromDate &&
            log.CreatedAt <= toDate &&
            (log.Type == ActivityType.CreateBHPost ||
             log.Type == ActivityType.BHComment ||
             log.Type == ActivityType.BHVote ||
             log.Type == ActivityType.BHLike ||
             log.Type == ActivityType.BHUnlike))
    {
        AddIncludes();
        ApplyOrderByDescending(log => log.CreatedAt);
    }

    private void AddIncludes()
    {
        Includes.Add(log => log.User);
        // Add any other necessary includes for BlackHat-related entities
        // For example, if you need to include the target post or comment:
        //Includes.Add(log => log.BHPost);
        // Includes.Add(log => log.BHComment);
    }
}


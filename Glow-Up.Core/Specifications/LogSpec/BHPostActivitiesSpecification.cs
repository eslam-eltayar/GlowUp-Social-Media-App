using Glow_Up.Core.Enums;
using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.LogSpec;
public class BHPostActivitiesSpecification : BaseSpecification<ActivityLog>
{
    public BHPostActivitiesSpecification(int postId)
        : base(log =>
            log.TargetId == postId &&
            (log.Type == ActivityType.CreateBHPost ||
             log.Type == ActivityType.BHComment ||
             log.Type == ActivityType.BHVote ||
             log.Type == ActivityType.BHLike ||
             log.Type == ActivityType.BHUnlike))
    {
        AddIncludes();
        ApplyOrderByDescending(log => log.CreatedAt);
    }

    public BHPostActivitiesSpecification(int postId, DateTime fromDate, DateTime toDate)
        : base(log =>
            log.TargetId == postId &&
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
        // Include any related BlackHat entities you need
        // For example:
        // Includes.Add(log => log.BHPost);
        // Includes.Add(log => log.BHComment);
    }
}


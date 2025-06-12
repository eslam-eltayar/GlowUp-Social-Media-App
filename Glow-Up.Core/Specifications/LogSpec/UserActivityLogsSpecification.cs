using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.LogSpec;
public class UserActivityLogsSpecification : BaseSpecification<ActivityLog>
{
    public UserActivityLogsSpecification(int userId)
        : base(log => log.UserId == userId)
    {
        ApplyOrderByDescending(log => log.CreatedAt);
        AddIncludes();
    }

    private void AddIncludes()
    {
        Includes.Add(log => log.User);
    }
}

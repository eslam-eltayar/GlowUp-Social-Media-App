using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.LogSpec;
public class RecentActivityLogsSpecification : BaseSpecification<ActivityLog>
{
    public RecentActivityLogsSpecification()
         : base(x => true) 
    {
        AddIncludes();
        ApplyOrderByDescending(x => x.CreatedAt);
    }

    //public RecentActivityLogsSpecification(DateTime fromDate, DateTime toDate)
    //: base(x => x.CreatedAt >= fromDate && x.CreatedAt <= toDate)
    //{
    //    AddIncludes();
    //    ApplyOrderByDescending(x => x.CreatedAt);
    //}

    private void AddIncludes()
    {
        Includes.Add(x => x.User);
    }
}

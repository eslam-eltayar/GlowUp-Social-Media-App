using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.User_Spec
{
    public class FollowersSpecification : BaseSpecification<Follow>
    {
        public FollowersSpecification(int userId)
            : base(f => f.FolloweeId == userId)
        {
            AddIncludes();

            ApplyOrderByDescending(f => f.Id);
        }

        private void AddIncludes()
        {
            Includes.Add(f => f.Followee);
            Includes.Add(f => f.Follower);
        }
    }
}

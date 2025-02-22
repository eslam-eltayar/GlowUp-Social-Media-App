using Glow_Up.Core.Models.BlackHat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.BlackHat
{
    public class BHCommentsForPost : BaseSpecification<BHComment>
    {
        public BHCommentsForPost(int postId)
            : base(c => c.BHPostId == postId)
        {
            AddIncludes();
            ApplyOrderByDescending(c => c.VoteCount);
        }

        private void AddIncludes()
        {
            Includes.Add(c => c.User);
            Includes.Add(c => c.BHPost);
        }
    }
}

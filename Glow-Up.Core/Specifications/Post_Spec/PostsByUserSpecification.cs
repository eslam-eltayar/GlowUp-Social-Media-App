using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.Post_Spec
{
    public class PostsByUserSpecification : BaseSpecification<Post>
    {
        public PostsByUserSpecification(int userId)
        : base(p => p.UserId == userId)
        {
            AddIncludes();
            ApplyOrderByDescending(p => p.Id);
        }
        private void AddIncludes()
        {
            Includes.Add(p => p.Comments);
            Includes.Add(p => p.Reactions);
            Includes.Add(p => p.MediaItems);
            Includes.Add(p => p.User);
        }
    }
}

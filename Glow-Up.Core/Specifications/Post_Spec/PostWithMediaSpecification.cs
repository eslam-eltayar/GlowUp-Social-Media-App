using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.Post_Spec
{
    public class PostWithMediaSpecification : BaseSpecification<Post>
    {
        public PostWithMediaSpecification(int postId)
            : base(p => p.Id == postId)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(p => p.MediaItems);
        }
    }
}

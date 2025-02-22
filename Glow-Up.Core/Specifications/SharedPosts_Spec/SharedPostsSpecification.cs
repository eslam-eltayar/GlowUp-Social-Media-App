using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.SharedPosts_Spec
{
    public class SharedPostsSpecification : BaseSpecification<SharedPost>
    {
        public SharedPostsSpecification(int userId)
            : base(sp => sp.UserId == userId)
        {
            AddIncludes();
            ApplyOrderByDescending(sp => sp.Id);
        }

        public SharedPostsSpecification()
        : base(sp => true) // Fetch all shared posts
        {
            AddIncludes();
        }

       
        private void AddIncludes()
        {
            Includes.Add(sp => sp.Post);
            Includes.Add(sp => sp.Post.User);
            Includes.Add(sp => sp.User);
            Includes.Add(sp => sp.Post.MediaItems);
        }
    }
}

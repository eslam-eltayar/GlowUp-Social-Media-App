using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.SharedPosts_Spec
{
    public class SharedPostsByUserSpecification : BaseSpecification<SharedPost>
    {
        public SharedPostsByUserSpecification(int userId)
         : base(sp => sp.UserId == userId)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(sp => sp.Post);
            Includes.Add(sp => sp.Post.MediaItems);
            Includes.Add(sp => sp.Post.User);
        }
    }
}

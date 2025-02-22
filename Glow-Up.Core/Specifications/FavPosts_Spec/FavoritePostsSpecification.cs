using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.FavPosts_Spec
{
    public class FavoritePostsSpecification : BaseSpecification<FavoritePost>
    {

        public FavoritePostsSpecification(int userId)
            : base(f => f.UserId == userId)
        {
            AddIncludes();
            ApplyOrderByDescending(f => f.Id);
        }
        private void AddIncludes()
        {
            Includes.Add(f => f.Post);
            Includes.Add(f => f.Post.User);
            Includes.Add(f => f.User);
            Includes.Add(fp => fp.Post.MediaItems);
        }
    }
}

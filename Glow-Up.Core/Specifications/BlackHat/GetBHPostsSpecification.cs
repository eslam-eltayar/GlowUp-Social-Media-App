using Glow_Up.Core.Enums;
using Glow_Up.Core.Models.BlackHat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.BlackHat
{
    public class GetBHPostsSpecification : BaseSpecification<BHPost>
    {
        public GetBHPostsSpecification(string? category)
        : base(c => (string.IsNullOrEmpty(category) || c.Category == Enum.Parse<Category>(category)))
        {
            ApplyOrderByDescending(x => x.Id);

            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(p => p.User);
            Includes.Add(p => p.Medias);
            Includes.Add(p => p.Comments);
            Includes.Add(p => p.Likes);
        }
    }
}

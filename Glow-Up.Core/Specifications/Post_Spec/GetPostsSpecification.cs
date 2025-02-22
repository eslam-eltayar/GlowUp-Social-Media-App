using Glow_Up.Core.Enums;
using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.Post_Spec
{
    public class GetPostsSpecification : BaseSpecification<Post>
    {
        public GetPostsSpecification(string feeling)
        : base(feeling == null ? null : p => p.Reactions.Any(r => r.Type == GetReactionTypeForFeeling(feeling)))
        {
            AddIncludes();

            ApplyOrderByDescending(p => p.Id);
            
            if (!string.IsNullOrEmpty(feeling))
            {
                ApplyOrderByReactionCount(feeling);
            }
        }


        private void AddIncludes()
        {
            Includes.Add(p => p.Comments);
            Includes.Add(p => p.Reactions);
            Includes.Add(p => p.MediaItems);
            Includes.Add(p => p.User);

        }

        private void ApplyOrderByReactionCount(string feeling)
        {
            var reactionType = GetReactionTypeForFeeling(feeling);

            ApplyOrderByDescending(p => p.Reactions.Count(r => r.Type == reactionType));
        }

        private static ReactType GetReactionTypeForFeeling(string? feeling = null)
        {
            return feeling.ToLower() switch
            {
                "happy" => ReactType.Touched,
                "sad" => ReactType.Funny,
                "relax" => ReactType.Chill,
                "motivated" => ReactType.Awesome,
                _ => throw new ArgumentException("Invalid feeling.")
            };
        }

    }
}

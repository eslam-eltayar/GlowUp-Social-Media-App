using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.Comment_Spec
{
    public class CommentsForPostSpecification : BaseSpecification<Comment>
    {
        public CommentsForPostSpecification(int postId)
            : base(c => c.PostId == postId /*&& c.ParentCommentId == null*/)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            
            Includes.Add(c => c.Replies);
            Includes.Add(c => c.User);
            Includes.Add(c => c.User.Replies);

            //Includes.Add(c => c.Replies.Select(r => r.User));

            //AddThenInclude(c => c.Replies, r => r.Replies);

            //if (Includes.Any())
            //{
            //    foreach (var include in Includes)
            //    {
            //        if (include is IIncludeQuery<Comment, ICollection<Comment>> includeQuery)
            //        {
            //            includeQuery.ThenInclude(c => c.Replies);
            //        }
            //    }
            //}
        }
    }
}

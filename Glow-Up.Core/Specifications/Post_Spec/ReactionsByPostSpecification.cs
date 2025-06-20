using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.Post_Spec;
public class ReactionsByPostSpecification : BaseSpecification<Reaction>
{
    public ReactionsByPostSpecification(int postId)
        : base(r => r.PostId == postId)
    {
        AddIncludes();
    }
    private void AddIncludes()
    {
        Includes.Add(r => r.User);
    }
}

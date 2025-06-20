using Glow_Up.Core.DTOs.Post;
using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.Post_Spec;
public class ReportedPostsSpecification : BaseSpecification<ReportPost>
{
    public ReportedPostsSpecification(int postId)
        : base(x=>x.PostId == postId) 
    {
      
    }
    
}

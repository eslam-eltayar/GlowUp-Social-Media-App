using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Specifications.User_Spec;
public class UserSearchSpecification : BaseSpecification<User>
{
    public UserSearchSpecification(string searchTerm)
        : base(u =>
            string.IsNullOrEmpty(searchTerm) ||
            u.FirstName.ToLower().Contains(searchTerm.ToLower()) ||
            u.LastName.ToLower().Contains(searchTerm.ToLower()))
    {
        ApplyOrderBy(u => u.FirstName);
    }
}

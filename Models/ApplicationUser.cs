using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }

        public string FullName { get
            {
                if (FirstName.Length > 0 && LastName.Length > 0)
                    return FirstName + " " + LastName;
                else if (FirstName.Length > 0)
                    return FirstName;
                else if (LastName.Length > 0)
                    return LastName;
                else
                    return UserName;
            }
        }
    }
}


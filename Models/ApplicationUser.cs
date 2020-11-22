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

        public string FullName
        {
            get
            {
                if (!String.IsNullOrEmpty(FirstName) && !String.IsNullOrEmpty(LastName))
                    return FirstName + " " + LastName;
                else if (!String.IsNullOrEmpty(FirstName))
                    return FirstName;
                else if (!String.IsNullOrEmpty(LastName))
                    return LastName;
                else
                    return UserName;
            }
        }
    }
}


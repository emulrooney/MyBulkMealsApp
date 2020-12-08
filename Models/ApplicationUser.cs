using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        [MaxLength(20)]
        [RegularExpression("/[A-Za-z -]*/")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [MaxLength(40)]
        [RegularExpression("/[A-Za-z -]*/")]
        public string LastName { get; set; }
        [MaxLength(40)]
        [RegularExpression("/[A-Za-z -]*/")]
        public string Location { get; set; }

        [Display(Name = "Name")]
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


using MyBulkMealsApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBulkMealsApp.Models
{
    public partial class UserItem : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }
        [DisplayName("Item Name")]
        public string ItemName { get; set; }
        [DisplayName("Creation Time")]
        public DateTime CreatedTime { get; set; }
        public string CreatorId { get; set; }

        [DisplayName("Verified Item")]
        public bool IsVerified { get; set; }
        [DisplayName("Visible to Public")]
        public bool IsPublic { get; set; }
        public DateTime? VerificationSubmissionTime { get; set; }
        public bool IsAmendment { get; set; }

        public ApplicationUser Creator { get; set; }

        public UserItem()
        {
            CreatedTime = DateTime.Now;
        }
    }
}

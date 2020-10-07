using MyBulkMealsApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBulkMealsApp.Models
{
    public partial class UserItem : IEntity
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public DateTime CreatedTime { get; set; }
        public int CreatorId { get; set; }
        public bool IsVerified { get; set; }
        public bool IsPublic { get; set; }
        public DateTime? VerificationSubmissionTime { get; set; }
        public bool IsAmendment { get; set; }

        public UserItem()
        {
            CreatedTime = DateTime.Now;
        }
    }
}

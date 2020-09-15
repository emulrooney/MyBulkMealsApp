using System;
using System.Collections.Generic;

namespace MyBulkMealsApp.Models
{
    public partial class UserItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public DateTime CreatedTime { get; set; }
        public int CreatorId { get; set; }
        public int? CopyOf { get; set; }
        public byte[] IsVerified { get; set; }
        public byte[] IsPublic { get; set; }
        public DateTime? VerificationSubmission { get; set; }
        public byte[] IsAmendment { get; set; }
    }
}

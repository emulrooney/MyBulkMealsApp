using System;
using System.Collections.Generic;

namespace MyBulkMealsApp.Models
{
    public partial class UserSavedItem
    {
        public int Id { get; set; }
        public int UserItemId { get; set; }
        public int SavedBy { get; set; }
    }
}

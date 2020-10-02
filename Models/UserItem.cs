using MyBulkMealsApp.Data;
using System;
using System.Collections.Generic;

namespace MyBulkMealsApp.Models
{
    public partial class UserItem : IEntity
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public DateTime CreatedTime { get; set; }
        public int CreatorId { get; set; }
        public int? RecipeId { get; set; }
        public int? IngredientId { get; set; }
        public byte[] IsVerified { get; set; }
        public byte[] IsPublic { get; set; }
        public DateTime? VerificationSubmission { get; set; }
        public byte[] IsAmendment { get; set; }

        public Recipe Recipe { get; set; }
        public Ingredient Ingredient { get; set; }

    }
}

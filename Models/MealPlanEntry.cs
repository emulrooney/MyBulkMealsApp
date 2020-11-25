using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBulkMealsApp.Models
{
    public partial class MealPlanEntry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}

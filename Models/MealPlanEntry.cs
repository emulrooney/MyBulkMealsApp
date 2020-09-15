using System;
using System.Collections.Generic;

namespace MyBulkMealsApp.Models
{
    public partial class MealPlanEntry
    {
        public int Id { get; set; }
        public byte[] IsException { get; set; }
        public byte Quantity { get; set; }
        public int MealPlanId { get; set; }
        public int RecipeId { get; set; }
        public byte Day { get; set; }
    }
}

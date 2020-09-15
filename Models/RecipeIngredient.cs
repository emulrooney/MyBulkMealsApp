using System;
using System.Collections.Generic;

namespace MyBulkMealsApp.Models
{
    public partial class RecipeIngredient
    {
        public int Id { get; set; }
        public int IngredientId { get; set; }
        public int RecipeId { get; set; }
        public double MeasurementAmount { get; set; }
    }
}

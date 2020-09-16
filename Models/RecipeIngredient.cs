using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBulkMealsApp.Models
{
    public partial class RecipeIngredient
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int IngredientId { get; set; }
        [Required]
        public int RecipeId { get; set; }
        public double MeasurementAmount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Models
{
    public partial class RecipeIngredient
    {
        [Key]
        public int Id { get; set; }

        public int IngredientId { get; set; }
        public int RecipeId { get; set; }

        public Ingredient Ingredient { get; set; }
        public Recipe Recipe { get; set; }

        public double MeasurementAmount { get; set; }
   }
}

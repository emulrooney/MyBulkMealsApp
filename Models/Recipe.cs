using MyBulkMealsApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MyBulkMealsApp.Models
{
    public partial class Recipe : UserItem
    {
        public string ImageUrl { get; set; }
        [Required]
        public int BaseServings { get; set; }
        public string Instructions { get; set; }
        public int Time { get; set; }
        public int Views { get; set; }

        public ICollection<RecipeIngredient> Ingredients { get; set; }

        public int TotalCalories { get
            {
                return (int)(Ingredients.Sum(i => i.Ingredient.Calories * i.MeasurementAmount));
            }
        }

        public int TotalProtein
        {
            get
            {
                return (int)(Ingredients.Sum(i => i.Ingredient.Protein * i.MeasurementAmount));
            }
        }

        public int TotalCarbs
        {
            get
            {
                return (int)(Ingredients.Sum(i => i.Ingredient.Carbs * i.MeasurementAmount));
            }
        }

        public int TotalFat
        {
            get
            {
                return (int)(Ingredients.Sum(i => i.Ingredient.Fat * i.MeasurementAmount));
            }
        }

    }
}

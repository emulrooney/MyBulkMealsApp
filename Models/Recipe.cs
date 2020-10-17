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
                int calories = 0;
                if (Ingredients != null)
                {
                    foreach (var i in Ingredients )
                    {
                        Console.WriteLine(i);
                        calories += (int)(i.Ingredient.Calories * i.MeasurementAmount);
                    }
                }
                return calories;
            }
        }

        public int TotalProtein
        {
            get
            {
                if (Ingredients != null)
                    return (int)(Ingredients.Sum(i => i.Ingredient.Protein * i.MeasurementAmount));
                return 0;
            }
        }

        public int TotalCarbs
        {
            get
            {
                if (Ingredients != null)
                    return (int)(Ingredients.Sum(i => i.Ingredient.Carbs * i.MeasurementAmount));
                return 0;
            }
        }

        public int TotalFat
        {
            get
            {
                if (Ingredients != null)
                    return (int)(Ingredients.Sum(i => i.Ingredient.Fat * i.MeasurementAmount));
                return 0;
            }
        }

    }
}

﻿using MyBulkMealsApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MyBulkMealsApp.Models
{
    public partial class Recipe : UserItem
    {
        [Required]
        [DisplayName("Base Servings")]
        public int BaseServings { get; set; }
        public string Instructions { get; set; }
        [DisplayName("Time (Minutes)")]
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
                        calories += (int)(i.Ingredient.Calories / i.Ingredient.BaseMeasurement * i.MeasurementAmount);
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
                    return (int)(Ingredients.Sum(i => i.Ingredient.Protein / i.Ingredient.BaseMeasurement * i.MeasurementAmount));
                return 0;
            }
        }

        public int TotalCarbs
        {
            get
            {
                if (Ingredients != null)
                    return (int)(Ingredients.Sum(i => i.Ingredient.Carbs / i.Ingredient.BaseMeasurement * i.MeasurementAmount));
                return 0;
            }
        }

        public int TotalFat
        {
            get
            {
                if (Ingredients != null)
                    return (int)(Ingredients.Sum(i => i.Ingredient.Fat / i.Ingredient.BaseMeasurement * i.MeasurementAmount));
                return 0;
            }
        }

    }
}

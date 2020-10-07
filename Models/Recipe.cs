using MyBulkMealsApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public ICollection<Ingredient> Ingredients { get; set; }
    }
}

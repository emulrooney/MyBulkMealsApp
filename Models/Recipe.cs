using System;
using System.Collections.Generic;

namespace MyBulkMealsApp.Models
{
    public partial class Recipe
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ImageUrl { get; set; }
        public byte? BaseServings { get; set; }
        public string Instructions { get; set; }
        public string Step { get; set; }
        public byte? Time { get; set; }
        public int? Views { get; set; }
    }
}

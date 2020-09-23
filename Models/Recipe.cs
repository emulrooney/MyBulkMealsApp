using MyBulkMealsApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBulkMealsApp.Models
{
    public partial class Recipe : IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ItemName { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public byte? BaseServings { get; set; }
        public string Instructions { get; set; }
        public byte? Time { get; set; }
        public int? Views { get; set; }
    }
}

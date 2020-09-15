using System;
using System.Collections.Generic;

namespace MyBulkMealsApp.Models
{
    public partial class Ingredient
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string MeasurementType { get; set; }
        public double BaseMeasurement { get; set; }
        public short? Calories { get; set; }
        public short? Protein { get; set; }
        public short? Carbs { get; set; }
        public short? Fat { get; set; }
    }
}

using MyBulkMealsApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBulkMealsApp.Models
{
    public partial class Ingredient : IEntity
    {
        public int Id { get; set; }
        [DisplayName("Item")]
        public string ItemName { get; set; }
        public string MeasurementType { get; set; }
        public double BaseMeasurement { get; set; }
        public short? Calories { get; set; }
        public short? Protein { get; set; }
        public short? Carbs { get; set; }
        public short? Fat { get; set; }

        public string DefaultMeasurement { 
            get 
            {
                return "" + BaseMeasurement + " " + MeasurementType;
            }
        }
    }
}

using MyBulkMealsApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBulkMealsApp.Models
{
    public partial class Ingredient : UserItem
    {
        public int MeasurementId { get; set; } = 0;

        [DisplayName("Measurement")]
        public double BaseMeasurement { get; set; }
        public short? Calories { get; set; }
        public short? Protein { get; set; }
        public short? Carbs { get; set; }
        public short? Fat { get; set; }
        public Measurement Measurement { get; set; }

        public string DefaultMeasurement { 
            get 
            {
                if (Measurement != null)
                    return "" + BaseMeasurement + Measurement.Symbol ?? "?";
                else
                    return "";
            }
        }

        public string LongMeasurement
        {
            get
            {
                if (Measurement != null)
                    return "" + BaseMeasurement + Measurement.Name;
                else
                    return "";
            }
        }
    }
}

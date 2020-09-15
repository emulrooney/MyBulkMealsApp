using System;
using System.Collections.Generic;

namespace MyBulkMealsApp.Models
{
    public partial class MealPlan
    {
        public int Id { get; set; }
        public string PlanName { get; set; }
        public int CreatorId { get; set; }
        public byte MealsPerDay { get; set; }
        public byte TotalDays { get; set; }
        public byte StartDay { get; set; }
        public byte EndDay { get; set; }
    }
}

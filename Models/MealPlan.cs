using System;
using System.Collections.Generic;

namespace MyBulkMealsApp.Models
{
    public partial class MealPlan
    {
        public int Id { get; set; }
        public string PlanName { get; set; }
        public int CreatorId { get; set; }
        public int MealsPerDay { get; set; }
        public int TotalDays { get; set; }
        public int StartDay { get; set; }
        public int EndDay { get; set; }

        public ICollection<MealPlanEntry> MealPlanEntries { get; set; }


    }
}

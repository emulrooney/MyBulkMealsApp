using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MyBulkMealsApp.Models
{
    public partial class MealPlan
    {
        public int Id { get; set; }
        [DisplayName("Plan Name")]
        public string PlanName { get; set; }
        public string CreatorId { get; set; }
        [DisplayName("Meals Per Day")]
        public int MealsPerDay { get; set; }
        [DisplayName("Plan Length")]
        public int TotalDays { get; set; }
        [DisplayName("Starts On")]
        public int StartDay { get; set; }
        [DisplayName("Ends On")]
        public int EndDay { get; set; }

        public ICollection<MealPlanEntry> MealPlanEntries { get; set; }

        public string StartsOn
        {
            get
            {
                return GetDayAsString(StartDay);
            }
        }

        public string EndsOn
        {
            get
            {
                return GetDayAsString(EndDay);
            }
        }

        public static string GetDayAsString(int day)
        {
            switch (day)
            {
                case 0:
                    return "Monday";
                case 1:
                    return "Tuesday";
                case 2:
                    return "Wednesday";
                case 3:
                    return "Thursday";
                case 4:
                    return "Friday";
                case 5:
                    return "Saturday";
                case 6:
                    return "Sunday";
                default:
                    return "";
            }

        }
    }
}

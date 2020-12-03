using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Data
{
    /// <summary>
    /// Minimal class to have static configuration info in one place. 
    /// </summary>
    public static class SimpleConfig
    {
        public static int VerificationNotificationTrigger { get; } = 20;
        public static int ItemsPerPage { get; } = 25;
    }
}

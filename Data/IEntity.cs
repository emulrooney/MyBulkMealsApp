using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Data
{
    public interface IEntity
    {
        int Id { get; set; }
        string ItemName { get; set; }

        string CreatorId { get; set; }
        DateTime CreatedTime { get; set; }

        bool IsVerified { get; set; }
        DateTime? VerificationSubmissionTime { get; set; }

    }
}

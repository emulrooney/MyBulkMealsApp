﻿using System;
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

        bool IsPublic { get; set; }
        bool IsAmendment { get; set; }
        int AmendmentCount { get; set; }
        int BasedOn { get; set; }
        bool IsVerified { get; set; }
        DateTime? VerificationSubmissionTime { get; set; }

        IEntity Copy(int id);
    }
}

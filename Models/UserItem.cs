using MyBulkMealsApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBulkMealsApp.Models
{
    public partial class UserItem : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }
        [DisplayName("Item Name")]
        public string ItemName { get; set; }
        [DisplayName("Creation Time")]
        public DateTime CreatedTime { get; set; }
        public string CreatorId { get; set; }

        [DisplayName("Verified Item")]
        public bool IsVerified { get; set; }
        [DisplayName("Visible to Public")]
        public bool IsPublic { get; set; }
        public DateTime? VerificationSubmissionTime { get; set; }
        public bool IsAmendment { get; set; }
        public int AmendmentCount { get; set; }
        public int BasedOn { get; set; }

        public ApplicationUser Creator { get; set; }

        /// <summary>
        /// Rather than using Creator.FullName, this is a little safer because it offers a fall back if a creator isn't set (for some reason)
        /// </summary>
        public string CreatorName { get
            {
                if (Creator != null)
                    return Creator.FullName;
                else
                    return "MBM Staff";

            }
        }

        public UserItem()
        {
            CreatedTime = DateTime.Now;
        }

        public IEntity Copy(int id)
        {
            var userItem = (UserItem)this.MemberwiseClone();
            userItem.Id = 0;
            userItem.BasedOn = id;
            userItem.CreatedTime = DateTime.Now;
            userItem.IsPublic = false;
            userItem.IsVerified = false;

            return userItem;
        }
    }
}

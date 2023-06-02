using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Category
    {
        public Category()
        {
            RatioCategoryCategoryId1Navigations = new HashSet<RatioCategory>();
            RatioCategoryCategoryId2Navigations = new HashSet<RatioCategory>();
            Users = new HashSet<User>();
        }

        public Guid CategoryId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<RatioCategory> RatioCategoryCategoryId1Navigations { get; set; }
        public virtual ICollection<RatioCategory> RatioCategoryCategoryId2Navigations { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}

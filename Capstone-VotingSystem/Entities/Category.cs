using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Category
    {
        public Category()
        {
            RatioCategoryRatioCategoryId1Navigations = new HashSet<RatioCategory>();
            RatioCategoryRatioCategoryId2Navigations = new HashSet<RatioCategory>();
            Users = new HashSet<User>();
        }

        public Guid CategoryId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<RatioCategory> RatioCategoryRatioCategoryId1Navigations { get; set; }
        public virtual ICollection<RatioCategory> RatioCategoryRatioCategoryId2Navigations { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}

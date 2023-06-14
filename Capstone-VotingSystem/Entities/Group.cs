using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Group
    {
        public Group()
        {
            Ratios = new HashSet<Ratio>();
            Users = new HashSet<User>();
        }

        public Guid GroupId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Ratio> Ratios { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}

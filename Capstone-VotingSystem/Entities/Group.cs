using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Group
    {
        public Group()
        {
            GroupUsers = new HashSet<GroupUser>();
            Ratios = new HashSet<Ratio>();
        }

        public Guid GroupId { get; set; }
        public string? Name { get; set; }
        public bool? IsVoter { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<GroupUser> GroupUsers { get; set; }
        public virtual ICollection<Ratio> Ratios { get; set; }
    }
}

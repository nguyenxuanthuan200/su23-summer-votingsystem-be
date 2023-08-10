using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Group
    {
        public Group()
        {
            Candidates = new HashSet<Candidate>();
            GroupUsers = new HashSet<GroupUser>();
            RatioGroupCandidates = new HashSet<Ratio>();
            RatioGroupVoters = new HashSet<Ratio>();
        }

        public Guid GroupId { get; set; }
        public string Name { get; set; } = null!;
        public bool IsVoter { get; set; }
        public string? Description { get; set; }
        public Guid CampaignId { get; set; }
        public bool IsStudentMajor { get; set; }

        public virtual Campaign Campaign { get; set; } = null!;
        public virtual ICollection<Candidate> Candidates { get; set; }
        public virtual ICollection<GroupUser> GroupUsers { get; set; }
        public virtual ICollection<Ratio> RatioGroupCandidates { get; set; }
        public virtual ICollection<Ratio> RatioGroupVoters { get; set; }
    }
}

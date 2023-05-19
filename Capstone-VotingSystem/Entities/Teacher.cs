using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Teacher
    {
        public Teacher()
        {
            VoteDetails = new HashSet<VoteDetail>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public double? AmountVote { get; set; }
        public double? Score { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? CampaignId { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual Department? Department { get; set; }
        public virtual ICollection<VoteDetail> VoteDetails { get; set; }
    }
}

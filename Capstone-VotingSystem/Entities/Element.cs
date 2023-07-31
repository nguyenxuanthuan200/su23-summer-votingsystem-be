using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Element
    {
        public Element()
        {
            VotingDetails = new HashSet<VotingDetail>();
        }

        public Guid ElementId { get; set; }
        public string? Content { get; set; }
        public bool? Status { get; set; }
        public Guid? QuestionId { get; set; }
        public decimal? Score { get; set; }

        public virtual Question? Question { get; set; }
        public virtual ICollection<VotingDetail> VotingDetails { get; set; }
    }
}

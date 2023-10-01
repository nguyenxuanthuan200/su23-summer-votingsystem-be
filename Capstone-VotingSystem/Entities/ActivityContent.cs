using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class ActivityContent
    {
        public Guid ActivityContentId { get; set; }
        public string Content { get; set; } = null!;
        public Guid ActivityId { get; set; }
        public Guid CandidateId { get; set; }

        public virtual Activity Activity { get; set; } = null!;
        public virtual Candidate Candidate { get; set; } = null!;
    }
}

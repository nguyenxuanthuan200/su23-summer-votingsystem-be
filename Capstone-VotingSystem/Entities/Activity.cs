using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Activity
    {
        public Guid ActivityId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public Guid? CandidateId { get; set; }

        public virtual Candidate? Candidate { get; set; }
    }
}

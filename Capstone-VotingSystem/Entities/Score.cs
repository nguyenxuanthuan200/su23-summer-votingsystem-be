using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Score
    {
        public Guid ScoreId { get; set; }
        public double? Point { get; set; }
        public Guid? CandidateId { get; set; }
        public Guid? StageId { get; set; }

        public virtual Candidate? Candidate { get; set; }
        public virtual Stage? Stage { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Score
    {
        public Guid ScoreId { get; set; }
        public double? Count { get; set; }

        public virtual CandidateProfile? CandidateProfile { get; set; }
    }
}

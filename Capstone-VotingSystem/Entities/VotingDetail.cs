using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class VotingDetail
    {
        public Guid VotingDetailId { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid ElementId { get; set; }
        public Guid VotingId { get; set; }

        public virtual Element Element { get; set; } = null!;
        public virtual Voting Voting { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class CampaignType
    {
        public CampaignType()
        {
            Campaigns = new HashSet<Campaign>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }
    }
}

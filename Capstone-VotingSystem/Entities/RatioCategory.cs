using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class RatioCategory
    {
        public RatioCategory()
        {
            VotingDetails = new HashSet<VotingDetail>();
        }

        public Guid RatioCategoryId { get; set; }
        public decimal? Percent { get; set; }
        public double? Ratio { get; set; }
        public bool? CheckRatio { get; set; }
        public Guid? CategoryId1 { get; set; }
        public Guid? CategoryId2 { get; set; }
        public Guid? CampaignId { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual Category? CategoryId1Navigation { get; set; }
        public virtual Category? CategoryId2Navigation { get; set; }
        public virtual ICollection<VotingDetail> VotingDetails { get; set; }
    }
}

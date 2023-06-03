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
        public double? Ratio { get; set; }
        public decimal? Percent { get; set; }
        public bool? CheckRatio { get; set; }
        public Guid? CampaignId { get; set; }
        public Guid? RatioCategoryId1 { get; set; }
        public Guid? RatioCategoryId2 { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual Category? RatioCategoryId1Navigation { get; set; }
        public virtual Category? RatioCategoryId2Navigation { get; set; }
        public virtual ICollection<VotingDetail> VotingDetails { get; set; }
    }
}

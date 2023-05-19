using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Qrcode
    {
        public Guid Id { get; set; }
        public string? Url { get; set; }
        public string? Img { get; set; }
        public bool? Status { get; set; }
        public Guid? CampaignId { get; set; }

        public virtual Campaign? Campaign { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Qrcode
    {
        public Guid QrId { get; set; }
        public Guid? CampaignId { get; set; }
        public string? Url { get; set; }
        public string? ImgQrcode { get; set; }
        public bool? Status { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Campaign
    {
        public Campaign()
        {
            Qrcodes = new HashSet<Qrcode>();
            Questions = new HashSet<Question>();
            Teachers = new HashSet<Teacher>();
        }

        public Guid Id { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public bool? Status { get; set; }
        public Guid? CampaignTypeId { get; set; }
        public Guid? CampusId { get; set; }

        public virtual CampaignType? CampaignType { get; set; }
        public virtual Campus? Campus { get; set; }
        public virtual ICollection<Qrcode> Qrcodes { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}

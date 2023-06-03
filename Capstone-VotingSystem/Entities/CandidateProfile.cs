using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class CandidateProfile
    {
        public CandidateProfile()
        {
            VotingDetails = new HashSet<VotingDetail>();
        }

        public Guid CandidateProfileId { get; set; }
        public string? NickName { get; set; }
        public DateTime? Dob { get; set; }
        public string? Image { get; set; }
        public string? Username { get; set; }
        public Guid? CampaignId { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual Score CandidateProfileNavigation { get; set; } = null!;
        public virtual User? UsernameNavigation { get; set; }
        public virtual ICollection<VotingDetail> VotingDetails { get; set; }
    }
}

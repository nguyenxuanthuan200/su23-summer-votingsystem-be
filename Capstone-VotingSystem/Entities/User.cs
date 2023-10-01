using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class User
    {
        public User()
        {
            Campaigns = new HashSet<Campaign>();
            Candidates = new HashSet<Candidate>();
            FeedBacks = new HashSet<FeedBack>();
            Forms = new HashSet<Form>();
            GroupUsers = new HashSet<GroupUser>();
            HistoryActions = new HashSet<HistoryAction>();
            Votings = new HashSet<Voting>();
        }

        public string UserId { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }
        public bool Status { get; set; }
        public int Permission { get; set; }

        public virtual Account UserNavigation { get; set; } = null!;
        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<Candidate> Candidates { get; set; }
        public virtual ICollection<FeedBack> FeedBacks { get; set; }
        public virtual ICollection<Form> Forms { get; set; }
        public virtual ICollection<GroupUser> GroupUsers { get; set; }
        public virtual ICollection<HistoryAction> HistoryActions { get; set; }
        public virtual ICollection<Voting> Votings { get; set; }
    }
}

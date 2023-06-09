﻿using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class User
    {
        public User()
        {
            ActionHistories = new HashSet<ActionHistory>();
            Campaigns = new HashSet<Campaign>();
            CandidateProfiles = new HashSet<CandidateProfile>();
            Feedbacks = new HashSet<Feedback>();
            Forms = new HashSet<Form>();
            Notifications = new HashSet<Notification>();
            Votings = new HashSet<Voting>();
        }

        public string UserName { get; set; } = null!;
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? RoleId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Role? Role { get; set; }
        public virtual Account UserNameNavigation { get; set; } = null!;
        public virtual ICollection<ActionHistory> ActionHistories { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<CandidateProfile> CandidateProfiles { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Form> Forms { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Voting> Votings { get; set; }
    }
}

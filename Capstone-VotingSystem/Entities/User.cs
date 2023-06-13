﻿using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class User
    {
        public User()
        {
            Candidates = new HashSet<Candidate>();
            FeedBacks = new HashSet<FeedBack>();
            HistoryActions = new HashSet<HistoryAction>();
            Votings = new HashSet<Voting>();
        }

        public string UserId { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }
        public bool? Status { get; set; }
        public Guid? GroupId { get; set; }

        public virtual Group? Group { get; set; }
        public virtual Account UserNavigation { get; set; } = null!;
        public virtual ICollection<Candidate> Candidates { get; set; }
        public virtual ICollection<FeedBack> FeedBacks { get; set; }
        public virtual ICollection<HistoryAction> HistoryActions { get; set; }
        public virtual ICollection<Voting> Votings { get; set; }
    }
}

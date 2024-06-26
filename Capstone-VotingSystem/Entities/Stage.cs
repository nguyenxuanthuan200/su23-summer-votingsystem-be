﻿using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Stage
    {
        public Stage()
        {
            Scores = new HashSet<Score>();
            Votings = new HashSet<Voting>();
        }

        public Guid StageId { get; set; }
        public string Title { get; set; } = null!;
        public string? Content { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Process { get; set; } = null!;
        public int LimitVote { get; set; }
        public bool IsUseForm { get; set; }
        public bool Status { get; set; }
        public Guid CampaignId { get; set; }
        public Guid? FormId { get; set; }

        public virtual Campaign Campaign { get; set; } = null!;
        public virtual Form? Form { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
        public virtual ICollection<Voting> Votings { get; set; }
    }
}

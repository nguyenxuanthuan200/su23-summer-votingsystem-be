﻿using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Stage
    {
        public Stage()
        {
            Votings = new HashSet<Voting>();
        }

        public Guid StageId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid? CampaignId { get; set; }
        public Guid? FormId { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual Form? Form { get; set; }
        public virtual ICollection<Voting> Votings { get; set; }
    }
}
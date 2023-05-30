﻿using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Question
    {
        public Question()
        {
            QuestionStages = new HashSet<QuestionStage>();
        }

        public Guid QuestionId { get; set; }
        public string? Ask { get; set; }
        public string? Description { get; set; }
        public Guid? CampaignId { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual ICollection<QuestionStage> QuestionStages { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Activity
    {
        public Activity()
        {
            ActivityContents = new HashSet<ActivityContent>();
        }

        public Guid ActivityId { get; set; }
        public string Title { get; set; } = null!;
        public string? Content { get; set; }

        public virtual ICollection<ActivityContent> ActivityContents { get; set; }
    }
}

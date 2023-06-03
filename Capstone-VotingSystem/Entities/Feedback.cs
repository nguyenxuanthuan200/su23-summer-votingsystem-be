using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Feedback
    {
        public Guid FeedbackId { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? Username { get; set; }

        public virtual User? UsernameNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Form
    {
        public Form()
        {
            FormStages = new HashSet<FormStage>();
        }

        public Guid FormId { get; set; }
        public string? Name { get; set; }
        public bool? Visibility { get; set; }
        public string? Username { get; set; }

        public virtual User? UsernameNavigation { get; set; }
        public virtual ICollection<FormStage> FormStages { get; set; }
    }
}

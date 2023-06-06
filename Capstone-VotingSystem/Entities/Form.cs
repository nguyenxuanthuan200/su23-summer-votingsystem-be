using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Form
    {
        public Form()
        {
            FormStages = new HashSet<FormStage>();
            Questions = new HashSet<Question>();
        }

        public Guid FormId { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public bool? Visibility { get; set; }

        public virtual User? UserNameNavigation { get; set; }
        public virtual ICollection<FormStage> FormStages { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}

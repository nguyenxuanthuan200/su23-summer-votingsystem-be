using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Form
    {
        public Form()
        {
            Questions = new HashSet<Question>();
            Stages = new HashSet<Stage>();
        }

        public Guid FormId { get; set; }
        public string Name { get; set; } = null!;
        public string Visibility { get; set; } = null!;
        public bool IsApprove { get; set; }
        public bool Status { get; set; }
        public Guid CategoryId { get; set; }
        public string UserId { get; set; } = null!;

        public virtual Category Category { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Stage> Stages { get; set; }
    }
}

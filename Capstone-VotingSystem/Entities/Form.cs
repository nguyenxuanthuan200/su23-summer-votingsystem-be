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
        public string? Name { get; set; }
        public string? Visibility { get; set; }
        public bool? Status { get; set; }
        public Guid? CategoryId { get; set; }
        public string? UserId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Stage> Stages { get; set; }
    }
}

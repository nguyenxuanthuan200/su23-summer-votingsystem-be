using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Type
    {
        public Type()
        {
            Questions = new HashSet<Question>();
        }

        public Guid TypeId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}

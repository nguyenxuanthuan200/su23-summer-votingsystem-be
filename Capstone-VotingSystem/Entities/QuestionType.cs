using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class QuestionType
    {
        public QuestionType()
        {
            Questions = new HashSet<Question>();
        }

        public Guid QuestionTypeId { get; set; }
        public string? TypeName { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}

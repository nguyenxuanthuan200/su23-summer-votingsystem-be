using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Element
    {
        public Guid ElementId { get; set; }
        public string? Text { get; set; }
        public Guid? QuestionId { get; set; }

        public virtual Question? Question { get; set; }
        public virtual Answer? Answer { get; set; }
    }
}

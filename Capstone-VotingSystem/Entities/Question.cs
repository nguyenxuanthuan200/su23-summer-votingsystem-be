using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Question
    {
        public Question()
        {
            Elements = new HashSet<Element>();
        }

        public Guid QuestionId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public bool? Status { get; set; }
        public Guid? FormId { get; set; }
        public Guid? TypeId { get; set; }

        public virtual Form? Form { get; set; }
        public virtual Type? Type { get; set; }
        public virtual ICollection<Element> Elements { get; set; }
    }
}

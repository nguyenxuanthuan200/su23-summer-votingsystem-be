using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
            Elements = new HashSet<Element>();
        }

        public Guid QuestionId { get; set; }
        public string? QuestionName { get; set; }
        public Guid? FormId { get; set; }
        public Guid? QuestionTypeId { get; set; }

        public virtual Form? Form { get; set; }
        public virtual QuestionType? QuestionType { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Element> Elements { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Student
    {
        public Student()
        {
            HistoryStudents = new HashSet<HistoryStudent>();
            VoteDetails = new HashSet<VoteDetail>();
        }

        public string Mssv { get; set; } = null!;
        public string? K { get; set; }
        public Guid? MajorId { get; set; }

        public virtual Major? Major { get; set; }
        public virtual ICollection<HistoryStudent> HistoryStudents { get; set; }
        public virtual ICollection<VoteDetail> VoteDetails { get; set; }
    }
}

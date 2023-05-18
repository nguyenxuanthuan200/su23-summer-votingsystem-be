using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Student
    {
        public Student()
        {
            VoteDetails = new HashSet<VoteDetail>();
        }

        public string Mssv { get; set; } = null!;
        public string? Email { get; set; }
        public string? Lock { get; set; }
        public Guid? MajorId { get; set; }
        public Guid? RoleId { get; set; }

        public virtual Major? Major { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<VoteDetail> VoteDetails { get; set; }
    }
}

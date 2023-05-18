using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Campus
    {
        public Campus()
        {
            Campaigns = new HashSet<Campaign>();
            Departments = new HashSet<Department>();
            Majors = new HashSet<Major>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Major> Majors { get; set; }
    }
}

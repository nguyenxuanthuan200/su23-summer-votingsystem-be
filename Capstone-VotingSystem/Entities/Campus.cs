using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Campus
    {
        public Campus()
        {
            Campaigns = new HashSet<Campaign>();
            CampusDepartments = new HashSet<CampusDepartment>();
            Majors = new HashSet<Major>();
        }

        public Guid CampusId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<CampusDepartment> CampusDepartments { get; set; }
        public virtual ICollection<Major> Majors { get; set; }
    }
}

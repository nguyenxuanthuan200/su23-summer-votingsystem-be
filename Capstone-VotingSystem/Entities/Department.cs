using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Department
    {
        public Department()
        {
            CampusDepartments = new HashSet<CampusDepartment>();
            Ratings = new HashSet<Rating>();
        }

        public Guid DepartmentId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<CampusDepartment> CampusDepartments { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}

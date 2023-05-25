using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Department
    {
        public Department()
        {
            DepartmentInCampuses = new HashSet<DepartmentInCampus>();
            Ratings = new HashSet<Rating>();
            Teachers = new HashSet<Teacher>();
        }

        public Guid DepartmentId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<DepartmentInCampus> DepartmentInCampuses { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}

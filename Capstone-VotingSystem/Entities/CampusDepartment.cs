using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class CampusDepartment
    {
        public CampusDepartment()
        {
            Teachers = new HashSet<Teacher>();
        }

        public Guid CampusDepartmentId { get; set; }
        public Guid? CampusId { get; set; }
        public Guid? DepartmentId { get; set; }

        public virtual Campus? Campus { get; set; }
        public virtual Department? Department { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}

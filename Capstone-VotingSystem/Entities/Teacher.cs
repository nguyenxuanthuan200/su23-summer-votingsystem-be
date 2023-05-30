using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Teacher
    {
        public Teacher()
        {
            TeacherCampaigns = new HashSet<TeacherCampaign>();
        }

        public Guid TeacherId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Img { get; set; }
        public Guid? CampusDepartmentId { get; set; }

        public virtual CampusDepartment? CampusDepartment { get; set; }
        public virtual ICollection<TeacherCampaign> TeacherCampaigns { get; set; }
    }
}

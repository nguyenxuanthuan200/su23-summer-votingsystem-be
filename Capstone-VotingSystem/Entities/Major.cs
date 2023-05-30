using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Major
    {
        public Major()
        {
            Ratings = new HashSet<Rating>();
            Students = new HashSet<Student>();
        }

        public Guid MajorId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? CampusId { get; set; }

        public virtual Campus? Campus { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Department
    {
        public Department()
        {
            Ratings = new HashSet<Rating>();
            Teachers = new HashSet<Teacher>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? CampusId { get; set; }

        public virtual Campus? Campus { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}

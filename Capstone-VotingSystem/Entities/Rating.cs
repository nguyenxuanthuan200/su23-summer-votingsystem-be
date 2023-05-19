using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Rating
    {
        public Guid Id { get; set; }
        public double? Ratio { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? MajorId { get; set; }

        public virtual Department? Department { get; set; }
        public virtual Major? Major { get; set; }
    }
}

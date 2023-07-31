using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class GroupUser
    {
        public Guid GroupUserId { get; set; }
        public string? UserId { get; set; }
        public Guid? GroupId { get; set; }

        public virtual Group? Group { get; set; }
        public virtual User? User { get; set; }
    }
}

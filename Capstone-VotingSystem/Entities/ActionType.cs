using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class ActionType
    {
        public ActionType()
        {
            ActionHistories = new HashSet<ActionHistory>();
        }

        public Guid ActionTypeId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<ActionHistory> ActionHistories { get; set; }
    }
}

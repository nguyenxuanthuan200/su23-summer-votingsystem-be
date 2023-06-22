using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class TypeAction
    {
        public TypeAction()
        {
            HistoryActions = new HashSet<HistoryAction>();
        }

        public Guid TypeActionId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<HistoryAction> HistoryActions { get; set; }
    }
}

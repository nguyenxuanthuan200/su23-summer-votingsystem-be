using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class TypeAction
    {
        public TypeAction()
        {
            HistoryMods = new HashSet<HistoryMod>();
            HistoryStudents = new HashSet<HistoryStudent>();
        }

        public Guid TypeActionId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<HistoryMod> HistoryMods { get; set; }
        public virtual ICollection<HistoryStudent> HistoryStudents { get; set; }
    }
}

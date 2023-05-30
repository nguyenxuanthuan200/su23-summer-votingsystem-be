using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class HistoryMod
    {
        public Guid HistoryId { get; set; }
        public string? Description { get; set; }
        public string? Username { get; set; }
        public Guid? TypeActionId { get; set; }

        public virtual TypeAction? TypeAction { get; set; }
        public virtual AccountMod? UsernameNavigation { get; set; }
    }
}

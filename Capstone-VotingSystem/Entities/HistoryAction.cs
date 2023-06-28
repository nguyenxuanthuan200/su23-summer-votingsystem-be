using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class HistoryAction
    {
        public Guid HistoryActionId { get; set; }
        public string? Description { get; set; }
        public DateTime? Time { get; set; }
        public Guid? TypeActionId { get; set; }
        public string? UserId { get; set; }

        public virtual TypeAction? TypeAction { get; set; }
        public virtual User? User { get; set; }
    }
}

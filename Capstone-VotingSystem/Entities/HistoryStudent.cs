using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class HistoryStudent
    {
        public Guid HistoryStudentId { get; set; }
        public string? Mssv { get; set; }
        public Guid? TypeActionId { get; set; }
        public string? Description { get; set; }

        public virtual Student? MssvNavigation { get; set; }
        public virtual TypeAction? TypeAction { get; set; }
    }
}

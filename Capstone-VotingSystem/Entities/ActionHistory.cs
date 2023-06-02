using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class ActionHistory
    {
        public Guid ActionHistoryId { get; set; }
        public string? Description { get; set; }
        public Guid? ActionTypeId { get; set; }
        public string? UserName { get; set; }

        public virtual ActionType? ActionType { get; set; }
        public virtual User? UserNameNavigation { get; set; }
    }
}

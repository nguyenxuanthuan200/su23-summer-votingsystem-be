using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class FormStage
    {
        public FormStage()
        {
            VotingDetails = new HashSet<VotingDetail>();
        }

        public Guid FormStageId { get; set; }
        public Guid? FormId { get; set; }

        public virtual Form? Form { get; set; }
        public virtual ICollection<VotingDetail> VotingDetails { get; set; }
    }
}

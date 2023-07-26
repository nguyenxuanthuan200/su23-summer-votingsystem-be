using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Category
    {
        public Category()
        {
            Campaigns = new HashSet<Campaign>();
            Forms = new HashSet<Form>();
        }

        public Guid CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<Form> Forms { get; set; }
    }
}

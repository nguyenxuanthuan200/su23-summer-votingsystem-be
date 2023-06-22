namespace Capstone_VotingSystem.Models.RequestModels.VotingDetailRequest
{
    public class CreateVotingDetailRequest
    {
        public Guid VotingDetailId { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? ElementId { get; set; }
        public Guid? VotingId { get; set; }
    }
}

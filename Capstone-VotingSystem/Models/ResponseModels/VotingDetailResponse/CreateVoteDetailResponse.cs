namespace Capstone_VotingSystem.Models.ResponseModels.VotingDetailResponse
{
    public class CreateVoteDetailResponse
    {
        public Guid VotingDetailId { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? ElementId { get; set; }
        public Guid? VotingId { get; set; }
    }
}

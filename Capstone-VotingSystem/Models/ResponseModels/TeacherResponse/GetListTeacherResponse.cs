namespace Capstone_VotingSystem.Models.ResponseModels.TeacherResponse
{
    public class GetListTeacherResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public double? AmountVote { get; set; }
        public double? Score { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? CampaignId { get; set; }
    }
}

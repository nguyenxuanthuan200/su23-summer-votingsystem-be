namespace Capstone_VotingSystem.Models.ResponseModels.GroupResponse
{
    public class StatisticalCandidateInGroupResponse
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public int TotalCandidateInGroup { get; set; }
    }
}

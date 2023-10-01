namespace Capstone_VotingSystem.Models.ResponseModels.GroupResponse
{
    public class StatisticalVoterMajorInGroupResponse
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public int TotalVoterMajorInGroup { get; set; }
    }
}

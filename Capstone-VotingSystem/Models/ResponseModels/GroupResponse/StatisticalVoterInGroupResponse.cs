namespace Capstone_VotingSystem.Models.ResponseModels.GroupResponse
{
    public class StatisticalVoterInGroupResponse
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public int TotalVoterInGroup { get; set; }
    }
}

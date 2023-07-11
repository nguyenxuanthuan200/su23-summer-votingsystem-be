namespace Capstone_VotingSystem.Models.RequestModels.ScoreRequest
{
    public class GetScoreByCampaginRequest
    {
        public Guid CampaginId { get; set; }
        public string UserId { get; set; }
    }
}

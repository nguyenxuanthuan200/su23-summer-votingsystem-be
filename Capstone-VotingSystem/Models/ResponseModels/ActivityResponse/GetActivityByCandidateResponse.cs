namespace Capstone_VotingSystem.Models.ResponseModels.ActivityResponse
{
    public class GetActivityByCandidateResponse
    {
        public Guid ActivityId { get; set; }
        public string Title { get; set; }
        public List<GetActivityContentResponse> ListContent { get; set; }
    }
}

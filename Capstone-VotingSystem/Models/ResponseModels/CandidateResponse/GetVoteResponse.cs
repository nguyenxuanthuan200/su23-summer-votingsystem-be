namespace Capstone_VotingSystem.Models.ResponseModels.CandidateResponse
{
    public class GetVoteResponse
    {
        public int? voteBM { get; set; } 
        public int? voteAM { get; set; } 
        public int voteRemaining { get; set; } 
    }
}

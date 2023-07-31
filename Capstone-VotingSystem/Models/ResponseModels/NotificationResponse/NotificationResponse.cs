namespace Capstone_VotingSystem.Models.ResponseModels.NotificationResponse
{
    public class NotificationResponse
    {
        public Guid NotificationId { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? IsRead { get; set; }
        public bool? Status { get; set; }
        public string? Username { get; set; }
    }
}

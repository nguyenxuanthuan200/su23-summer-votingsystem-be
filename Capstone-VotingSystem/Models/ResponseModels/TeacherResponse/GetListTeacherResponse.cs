namespace Capstone_VotingSystem.Models.ResponseModels.TeacherResponse
{
    public class GetListTeacherResponse
    {
        public Guid TeacherId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Img { get; set; }
        public Guid? CampusDepartmentId { get; set; }
    }
}

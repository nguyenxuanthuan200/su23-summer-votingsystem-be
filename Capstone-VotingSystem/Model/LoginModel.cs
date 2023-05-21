using System.ComponentModel.DataAnnotations;

namespace Capstone_VotingSystem.Model
{
    public class LoginModel
    {
        [Required]
        public string? UserName { get; set; }

        [Required, MaxLength(255)]
        public string? Password { get; set; }

    }
}

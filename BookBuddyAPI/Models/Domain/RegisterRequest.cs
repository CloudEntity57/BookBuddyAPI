using System.ComponentModel.DataAnnotations;

namespace BookBuddyAPI.Models.Domain
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [MinLength(3)]
        public string UserName { get; set; } = null!;
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = null!;
    }

}
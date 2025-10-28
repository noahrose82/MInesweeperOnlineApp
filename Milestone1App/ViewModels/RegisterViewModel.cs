using System.ComponentModel.DataAnnotations;

namespace Milestone1App.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Range(13, 120, ErrorMessage = "Age must be between 13 and 120.")]
        public int Age { get; set; }

        [Required]
        [StringLength(10)]
        public string Sex { get; set; } = string.Empty;

        [Required]
        [StringLength(2, ErrorMessage = "Use 2-letter state code.")]
        public string State { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty;

        
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

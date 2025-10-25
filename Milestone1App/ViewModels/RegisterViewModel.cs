using System.ComponentModel.DataAnnotations;

namespace Milestone1App.ViewModels
{
    /// <summary>
    /// ViewModel for user registration. Collects user input from the Register form. This does not map directly to the
    /// database — instead, data is validated, transformed, and then saved into the User entity.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Age of the user. Must be between 13 and 120.
        /// </summary>
        [Range(13, 120, ErrorMessage = "Age must be between 13 and 120.")]
        public int Age { get; set; }

        /// <summary>
        /// Email address of the user. Must be unique and valid format.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = "";

        /// <summary>
        /// First name of the new user.
        /// </summary>
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; } = "";

        /// <summary>
        /// Last name of the new user.
        /// </summary>
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; } = "";

        /// <summary>
        /// Plain-text password entered during registration. This will be hashed before being saved to the database.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = "";

        /// <summary>
        /// Sex of the user (e.g., Male, Female, Other).
        /// </summary>
        [Required(ErrorMessage = "Sex is required.")]
        public string Sex { get; set; } = "";

        /// <summary>
        /// Two-letter state code (e.g., TX, AZ).
        /// </summary>
        [Required(ErrorMessage = "State is required.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Use 2-letter state code.")]
        public string State { get; set; } = "";

        /// <summary>
        /// Desired username for login. Must be unique.
        /// </summary>
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = "";
    }
}
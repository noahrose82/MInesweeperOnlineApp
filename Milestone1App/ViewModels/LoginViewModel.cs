using System.ComponentModel.DataAnnotations;

namespace Milestone1App.ViewModels
{
    /// <summary>
    /// ViewModel for logging into the system. Collects username and password input from the Login form.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Plain-text password entered during login. Will be verified against the stored hashed password in the
        /// database.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        /// <summary>
        /// Username entered by the user.
        /// </summary>
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = "";
    }
}
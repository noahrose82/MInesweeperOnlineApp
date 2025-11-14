using System.ComponentModel.DataAnnotations;

namespace Milestone1App.Models
{
    /// <summary>
    /// Represents a single application user stored in the database. This class maps directly to the Users table in SQL
    /// Server.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The user's age. Must be between 13 and 120 years old.
        /// </summary>
        [Range(13, 120)]
        public int Age { get; set; }

        /// <summary>
        /// The user's email address. Required, must be unique, and valid email format.
        /// </summary>
        [Required, EmailAddress, StringLength(256)]
        public string Email { get; set; } = "";

        /// <summary>
        /// The user's first name. Required and limited to 50 characters.
        /// </summary>
        [Required, StringLength(50)]
        public string FirstName { get; set; } = "";

        /// <summary>
        /// Primary key for the user (auto-incrementing).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The user's last name. Required and limited to 50 characters.
        /// </summary>
        [Required, StringLength(50)]
        public string LastName { get; set; } = "";

        /// <summary>
        /// Hashed and salted password string. We never store plain text passwords in the database.
        /// </summary>
        [Required]
        public string PasswordHash { get; set; } = "";

        /// <summary>
        /// The user's sex (e.g., Male, Female, Other). Stored as text, limited to 10 characters.
        /// </summary>
        [Required, StringLength(10)]
        public string Sex { get; set; } = "";

        /// <summary>
        /// Two-letter state code (e.g., TX, AZ). Required and exactly 2 characters.
        /// </summary>
        [Required, StringLength(2)]
        public string State { get; set; } = "";

        /// <summary>
        /// The username chosen by the user. Required, must be unique, and up to 50 characters.
        /// </summary>
        [Required, StringLength(50)]
        public string Username { get; set; } = "";
    }
}
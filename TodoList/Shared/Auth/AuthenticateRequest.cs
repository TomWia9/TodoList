using System.ComponentModel.DataAnnotations;

namespace TodoList.Shared.Auth
{
    /// <summary>
    /// User's name and password
    /// </summary>
    public class AuthenticateRequest
    {
        /// <summary>
        /// The name of the user 
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(20, ErrorMessage = "The Username field may contain at most 20 characters.")]
        [MinLength(3, ErrorMessage = "The Username field must contain at least 3 characters.")]
        public string Username { get; set; }

        /// <summary>
        /// The password of the user 
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(20, ErrorMessage = "The Password field may contain at most 20 characters.")]
        [MinLength(5, ErrorMessage = "The Password field must contain at least 5 characters.")]
        public string Password { get; set; }
    }
}

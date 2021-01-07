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
        [MaxLength(20)]
        public string Username { get; set; }

        /// <summary>
        /// The password of the user 
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(20)]
        [MinLength(5)]
        public string Password { get; set; }
    }
}

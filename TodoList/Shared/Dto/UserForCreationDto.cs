using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Shared.Dto
{
    public class UserForCreationDto
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

        /// <summary>
        /// The confirmation of user password
        /// </summary>
        [DisplayName("Confirm password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Confirm Password field is required.")]
        [MaxLength(20, ErrorMessage = "The Confirm Password field may contain at most 20 characters.")]
        [MinLength(5, ErrorMessage = "The Confirm Password field must contain at least 5 characters.")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

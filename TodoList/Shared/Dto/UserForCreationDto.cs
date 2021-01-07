using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Shared.Dto
{
    public class UserForCreationDto
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

        /// <summary>
        /// The confirmation of user password
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(20)]
        [MinLength(5)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

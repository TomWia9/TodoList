using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Shared.Dto
{
    public abstract class TodoForManipulationDto
    {
        /// <summary>
        /// The title of todo
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(100, ErrorMessage = "The Title field may contain at most 100 characters.")]
        public string Title { get; set; }
        /// <summary>
        /// The description of todo
        /// </summary>
        [MaxLength(500, ErrorMessage = "The Description field may contain at most 500 characters.")]
        public string Description { get; set; }
    }
}

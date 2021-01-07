using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Shared.Dto
{
    public class ListOfTodosForCreationDto : ListOfTodosForManipulationDto
    {
        /// <summary>
        /// The id of user which this todo list belongs
        /// </summary>
        [Required]
        public int UserId { get; set; }
    }
}

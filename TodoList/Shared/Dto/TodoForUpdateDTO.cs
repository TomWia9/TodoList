using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Shared.Dto
{
    public class TodoForUpdateDto : TodoForManipulationDto
    {
        /// <summary>
        /// Todo is done status
        /// </summary>
        public bool IsDone { get; set; }
    }
}

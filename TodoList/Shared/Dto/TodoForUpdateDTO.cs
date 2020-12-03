using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Shared.Dto
{
    public class TodoForUpdateDto : TodoForManipulationDto
    {
        public bool IsDone { get; set; }
    }
}

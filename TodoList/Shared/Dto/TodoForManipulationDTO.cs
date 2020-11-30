using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Shared.Dto
{
    public abstract class TodoForManipulationDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}

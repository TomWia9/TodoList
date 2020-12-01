using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Shared.Dto
{
    public abstract class  ListOfTodosForManipulationDTO
    {
        public string Title { get; set; }
        public IEnumerable<TodoDTO> Todos { get; set; }
    }
}

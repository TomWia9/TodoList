using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Shared.Dto
{
    public class ListOfTodosDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<TodoDTO> Todos { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Server.Models
{
    public class ListOfTodos
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<Todo> Todos { get; set; }
    }
}

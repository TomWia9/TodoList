using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Shared.Enums;

namespace TodoList.Server.Models
{
    public class Todo
    {
        public Todo()
        {
            DateAdded = DateTime.Now;
            Color = TodoColor.light;
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public DateTime DateAdded { get; set; }
        public TodoColor Color { get; set; }
        public int ListOfTodosId { get; set; }
        public ListOfTodos ListOfTodos { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Server.Models
{
    public class Todo
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)] //this can't be done in fluent api
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public int ListOfTodosId { get; set; }
        public ListOfTodos ListOfTodos { get; set; }
    }
}

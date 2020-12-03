using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Server.Models
{
    public class ListOfTodos
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)] //this can't be done in fluent api

        public string Title { get; set; }
        public IEnumerable<Todo> Todos { get; set; }
    }
}

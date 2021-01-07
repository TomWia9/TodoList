﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Shared.Dto
{
    /// <summary>
    /// Todo list with Id, Title and Todos fields
    /// </summary>
    public class ListOfTodosDto
    {
        /// <summary>
        /// The id of todo list
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The title of todo list
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The list of todos belonging to this todo list
        /// </summary>
        public IEnumerable<TodoDto> Todos { get; set; }
    }
}

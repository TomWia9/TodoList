using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Shared.Enums;

namespace TodoList.Shared.Dto
{
    /// <summary>
    /// Todo with Id, Title, Description, IsDone, DateAdded, Color and ListOfTodosId fields
    /// </summary>
    public class TodoDto
    {
        /// <summary>
        /// The id of todo
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The title of todo
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The description of todo
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Todo is done status
        /// </summary>
        public bool IsDone { get; set; }
        /// <summary>
        /// The date of todo creation
        /// </summary>
        public DateTime DateAdded { get; set; }
        /// <summary>
        /// Color of todo
        /// </summary>
        public TodoColor Color { get; set; }
        /// <summary>
        /// The id of todo list which this todo belongs
        /// </summary>
        public int ListOfTodosId { get; set; }

    }
}

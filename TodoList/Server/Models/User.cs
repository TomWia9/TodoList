using System.Collections.Generic;

namespace TodoList.Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public IEnumerable<ListOfTodos> ListOfTodos { get; set; }

    }
}

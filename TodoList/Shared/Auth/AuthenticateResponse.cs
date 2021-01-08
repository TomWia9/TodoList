using System.Runtime.Serialization;
using TodoList.Shared.Dto;

namespace TodoList.Shared.Auth
{
    /// <summary>
    /// User with Id, Login and Token fields
    /// </summary>
    public class AuthenticateResponse
    {
        /// <summary>
        /// The id of the user
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name of the user 
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The token
        /// </summary>
        public string Token { get; set; }
    }
}

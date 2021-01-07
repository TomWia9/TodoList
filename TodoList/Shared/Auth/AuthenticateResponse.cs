using System.Runtime.Serialization;
using TodoList.Shared.Dto;

namespace TodoList.Shared.Auth
{
    /// <summary>
    /// User with Id, Login and Token fields
    /// </summary>
    [DataContract]
    public class AuthenticateResponse
    {
        /// <summary>
        /// The id of the user
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// The name of the user 
        /// </summary>
        [DataMember]
        public string Username { get; set; }
        /// <summary>
        /// The token
        /// </summary>
        [DataMember]
        public string Token { get; set; }

        public AuthenticateResponse(UserDto user, string token)
        {
            Id = user.Id;
            Username = user.Username;
            Token = token;
        }
    }
}

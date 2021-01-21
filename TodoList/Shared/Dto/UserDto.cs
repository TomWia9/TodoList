namespace TodoList.Shared.Dto
{
    /// <summary>
    /// User with Id and Username fields
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// The id of user
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name of user
        /// </summary>
        public string Username { get; set; }
    }
}

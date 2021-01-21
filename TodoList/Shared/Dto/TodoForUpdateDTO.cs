using TodoList.Shared.Enums;

namespace TodoList.Shared.Dto
{
    public class TodoForUpdateDto : TodoForManipulationDto
    {
        /// <summary>
        /// Todo is done status
        /// </summary>
        public bool IsDone { get; set; }
        /// <summary>
        /// Color of todo
        /// </summary>
        public TodoColor Color { get; set; }

    }
}

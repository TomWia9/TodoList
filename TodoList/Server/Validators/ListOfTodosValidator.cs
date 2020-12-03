using FluentValidation;
using TodoList.Shared.Dto;

namespace TodoList.Server.Validators
{
    public abstract class ListOfTodosValidator<T> : AbstractValidator<T> where T : ListOfTodosForManipulationDto
    {
        protected ListOfTodosValidator()
        {
            RuleFor(l => l.Title).NotEmpty().MaximumLength(100);
        }
    }
}
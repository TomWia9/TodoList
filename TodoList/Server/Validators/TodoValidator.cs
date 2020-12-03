using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using TodoList.Shared.Dto;

namespace TodoList.Server.Validators
{
    public abstract class TodoValidator<T> : AbstractValidator<T> where T: TodoForManipulationDto
    {
        protected TodoValidator()
        {
            RuleFor(t => t.Title).NotEmpty().MaximumLength(100);
            RuleFor(t => t.Description).MaximumLength(500);
        }
    }
}

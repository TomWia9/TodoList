using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Server.Models;
using TodoList.Shared.Dto;

namespace TodoList.Server.Data.Profiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<Todo, TodoDto>();
            CreateMap<TodoForCreationDto, Todo>();
            CreateMap<TodoForUpdateDto, Todo>();
            CreateMap<Todo, TodoForUpdateDto>();
        }
    }
}

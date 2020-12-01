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
            CreateMap<Todo, TodoDTO>();
            CreateMap<TodoForCreationDTO, Todo>();
            CreateMap<TodoForUpdateDTO, Todo>();
            CreateMap<Todo, TodoForUpdateDTO>();
        }
    }
}

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Server.Models;
using TodoList.Shared.Dto;

namespace TodoList.Server.Data.Profiles
{
    public class ListOfTodosProfile : Profile
    {
        public ListOfTodosProfile()
        {
            CreateMap<ListOfTodos, ListOfTodosDTO>();
            CreateMap<ListOfTodosForCreationDTO, ListOfTodos>();
            CreateMap<ListOfTodosForUpdateDTO, ListOfTodos>();
            CreateMap<ListOfTodos, ListOfTodosForUpdateDTO>();
        }
    }
}

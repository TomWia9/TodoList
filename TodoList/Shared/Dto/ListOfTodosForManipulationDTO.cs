using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Shared.Dto
{
    public abstract class  ListOfTodosForManipulationDto
    {
        public string Title { get; set; }

    }
}

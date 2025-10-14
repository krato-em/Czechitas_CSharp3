using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Domain.Models;

namespace ToDoList.Domain.DTOs
{
    public record ToDoItemGetResponseDto (int Id, string Name, string Description, bool IsCompleted)
    {
        public static ToDoItemGetResponseDto FromDomain(ToDoItem item) => new(item.ToDoItemId, item.Name, item.Description, item.IsCompleted);
    }
}

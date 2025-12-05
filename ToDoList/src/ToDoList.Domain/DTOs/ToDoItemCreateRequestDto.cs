namespace ToDoList.Domain.DTOs;

using System;
using ToDoList.Domain.Models;

public record ToDoItemCreateRequestDto(string Name, string Description, bool IsCompleted, int? CategoryId)
{
    public ToDoItem ToDomain() => new()
    {
        Name = Name,
        Description = Description,
        IsCompleted = IsCompleted,
        CategoryId = CategoryId
    };
}

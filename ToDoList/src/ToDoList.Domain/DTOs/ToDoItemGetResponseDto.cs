namespace ToDoList.Domain.DTOs;

using System;
using ToDoList.Domain.Models;
public record ToDoItemGetResponseDto(int Id, string Name, string Description, bool IsCompleted, int? CategoryId, string? CategoryName) //let client know the Id
{
    public static ToDoItemGetResponseDto FromDomain(ToDoItem item) => new(item.ToDoItemId, item.Name, item.Description, item.IsCompleted, item.CategoryId, item.Category?.Name);
}

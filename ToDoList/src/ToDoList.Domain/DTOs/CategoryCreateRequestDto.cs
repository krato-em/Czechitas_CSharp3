using System;
using ToDoList.Domain.Models;

namespace ToDoList.Domain.DTOs;

public record CategoryCreateRequestDto(string CategoryName)
{
    public Category ToDomain() => new()
    {
        Name = CategoryName
    };
}

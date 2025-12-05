using System;
using ToDoList.Domain.Models;

namespace ToDoList.Domain.DTOs;

public record CategoryUpdateRequestDto(string Name)
{
    public Category ToDomain() => new() { Name = Name};
}

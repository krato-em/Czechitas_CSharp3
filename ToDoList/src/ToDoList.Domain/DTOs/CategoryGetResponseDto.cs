using System;
using System.Runtime.CompilerServices;
using ToDoList.Domain.Models;

namespace ToDoList.Domain.DTOs;

public record CategoryGetResponseDto(int Id, string Name)
{
    public static CategoryGetResponseDto FromDomain(Category category) => new(category.CategoryId, category.Name);
}

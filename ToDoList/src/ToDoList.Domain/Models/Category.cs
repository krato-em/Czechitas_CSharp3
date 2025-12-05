using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Domain.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }
    [Length(1, 250)]
    public string Name { get; set; }
    public ICollection<ToDoItem> ToDoItems { get; set; } = new List<ToDoItem>(); // toto musi byt inicializovano, aby jsme se vyhli null references
}

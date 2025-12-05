using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Domain.Models;

public class ToDoItem
{
    [Key] // this specifies that ToDoItemId is a primary key
    public int ToDoItemId { get; set; }
    [Length(1, 50)] // aspon jeden znak, max 50
    public string Name { get; set; }
    [StringLength(250)] // klidne zadny znak, max 250
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    [ForeignKey(nameof(Category))]  // toto mozna je redundantni - EF je chytre a doplni si to samo
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
}

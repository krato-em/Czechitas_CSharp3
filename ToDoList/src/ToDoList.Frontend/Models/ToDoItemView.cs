using System.ComponentModel.DataAnnotations;
namespace ToDoList.Frontend.Views;
// public record ToDoItemView
// {
//     public required int ToDoItemId { get; set; }
//     [Length(1, 50)]
//     public required string Name { get; set; }
//     [StringLength(250)]
//     public required string Description { get; set; }
//     public required bool IsCompleted { get; set; }
// }

public class ToDoItemView
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
}

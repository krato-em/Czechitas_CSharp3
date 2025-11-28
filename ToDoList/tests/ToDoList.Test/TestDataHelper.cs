using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

namespace ToDoList.Test
{
    public static class TestDataHelper
    {
        public static ToDoItem toDoItem1 => new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Test Item 1",
            Description = "This is item 1",
            IsCompleted = false
        };

        public static ToDoItem toDoItem2 => new ToDoItem
        {
            ToDoItemId = 2,
            Name = "Test Item 2",
            Description = "This is item 2",
            IsCompleted = false
        };

        public static ToDoItem toDoItem3 => new ToDoItem
        {
            ToDoItemId = 3,
            Name = "Test Item 3",
            Description = "This is item 3",
            IsCompleted = true
        };

        public static void SeedTestData(ToDoItemsRepository repository)
        {
            repository.AddItemToStorage(toDoItem1);
            repository.AddItemToStorage(toDoItem2);
            repository.AddItemToStorage(toDoItem3);
        }
        public static void ClearTestData(ToDoItemsRepository repository)
        {
            repository.ClearStorage();
        }
    }
}

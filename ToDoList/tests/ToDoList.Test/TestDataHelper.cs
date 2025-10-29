using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.VisualBasic;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

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
            IsCompleted = false
        };

        public static void SeedTestData(ToDoItemsController controller)
        {
            controller.AddItemToStorage(toDoItem1);
            controller.AddItemToStorage(toDoItem2);
            controller.AddItemToStorage(toDoItem3);
        }
        public static void ClearTestData(ToDoItemsController controller)
        {
            controller.ClearStorage();
        }
    }
}

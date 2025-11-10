namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using ToDoList.Test;
using ToDoList.Persistence;

[Collection("Sequential")]
public class GetTests
{
    [Fact]
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Test Item 1",
            Description = "Popis",
            IsCompleted = false
        };
        var todoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "Test Item 2",
            Description = "Popis",
            IsCompleted = false
        };
        var context = new ToDoItemsContext("DataSource=../../../IntegrationTests/data/localdb_test.db");
        var controller = new ToDoItemsController(context);
        TestDataHelper.ClearTestData(controller);
        controller.AddItemToStorage(todoItem1);
        controller.AddItemToStorage(todoItem2);

        // Act

        var result = controller.Read();
        var value = ActionResultExtensions.GetValue(result);

        // Assert
        Assert.NotNull(value);
        var firstToDo = value.First();
        Assert.Equal(todoItem1.ToDoItemId, firstToDo.Id);
        Assert.Equal(todoItem1.Name, firstToDo.Name);
        Assert.Equal(todoItem1.Description, firstToDo.Description);
        Assert.Equal(todoItem1.IsCompleted, firstToDo.IsCompleted);
    }

    [Fact]
    public void Get_ItemById_ReturnItem()
    {
        // Arrange
        var context = new ToDoItemsContext("DataSource=../../../IntegrationTests/data/localdb_test.db");
        var controller = new ToDoItemsController(context);
        TestDataHelper.ClearTestData(controller);
        TestDataHelper.SeedTestData(controller);

        // Act
        var id = 2;
        var result = controller.ReadById(id);
        var value = ActionResultExtensions.GetValue(result);

        // Assert
        Assert.Equal(id, value.Id);
        Assert.Equal(TestDataHelper.toDoItem2.Name, value.Name);
        Assert.Equal(TestDataHelper.toDoItem2.Description, value.Description);
        Assert.Equal(TestDataHelper.toDoItem2.IsCompleted, value.IsCompleted);
    }

    [Fact]
    public void Get_ItemByInvaliId_ReturnErrorMessage()
    {
        // Arrange
        var context = new ToDoItemsContext("DataSource=../../../IntegrationTests/data/localdb_test.db");
        var controller = new ToDoItemsController(context);
        TestDataHelper.ClearTestData(controller);
        TestDataHelper.SeedTestData(controller);

        // Act
        var invalidId = controller.GetStoredToDoItemsId().Last() + 2;
        var result = controller.ReadById(invalidId);

        // Assert
        Assert.NotNull(result.Result);
        var objectResult = result.Result as ObjectResult;
        Assert.Equal(500, objectResult.StatusCode);

        var actualItems = controller.GetStoredToDoItems();
        Assert.DoesNotContain(actualItems, item => item.ToDoItemId == invalidId);
    }
}

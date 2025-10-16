namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

public class GetTests
{
    [Fact]
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Test Item",
            Description = "Popis",
            IsCompleted = false
        };
        var todoItem2 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Test Item",
            Description = "Popis",
            IsCompleted = false
        };
        var controller = new ToDoItemsController();
        controller.AddItemToStorage(todoItem1);
        controller.AddItemToStorage(todoItem2);

        // Act
        var result = controller.Read();
        var value = result;

        // Assert
        Assert.NotNull(value);
    }

    [Fact]
    public void DeleteTest()
    {
        ToDoItemsController.items.Clear();

        // Arrange
        var controller = new ToDoItemsController();
        ToDoItemsController.items.Add(new ToDoItem
        {
            ToDoItemId = 1,
            Name = "ItemToDelete",
            Description = "TestDelete",
            IsCompleted = false
        });

        // Act
        var result = controller.DeleteByid(1);
        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.DoesNotContain(ToDoItemsController.items, item => item.ToDoItemId == 1);
    }

    [Fact]
    public void CreateTest()
    {
        ToDoItemsController.items.Clear();

        // Arrange
        var controller = new ToDoItemsController();
        var request = new ToDoItemCreateRequestDto("addImte", "addDesc", true);

        var actionResult = controller.Create(request);
        var result = actionResult as CreatedAtActionResult;

        Assert.NotNull(result);
        Assert.Equal(nameof(ToDoItemsController.ReadById), result.ActionName);

        var dto = result.Value as ToDoItemGetResponseDto;
        Assert.NotNull(dto);
        Assert.Equal("addImte", dto.Name);
        Assert.Equal("addDesc", dto.Description);
        Assert.Equal(request.IsCompleted, dto.IsCompleted);
        Assert.Equal(1, dto.Id);

        Assert.Single(ToDoItemsController.items);
        Assert.Equal(1, ToDoItemsController.items[0].ToDoItemId);
    }
}

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

namespace ToDoList.Test;

[Collection("Sequential")]
public class UpdateTests
{
    [Fact]
    public void UpdateById_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var context = new ToDoItemsContext("DataSource=../../../IntegrationTests/data/localdb_test.db");
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);
        TestDataHelper.ClearTestData(repository);
        TestDataHelper.SeedTestData(repository);

        var existingToDoItemsId = repository.GetStoredToDoItemsId();
        var existingId = existingToDoItemsId.First();

        var updatedName = "Updated Item";
        var updatedDescription = "This item was updated";
        bool updatedIsCompleted = true;
        var toDoItemDto = new ToDoItemUpdateRequestDto(updatedName, updatedDescription, updatedIsCompleted, null);

        // Act
        var result = controller.UpdateById(existingId, toDoItemDto).Result;

        // Assert
        var storedItems = repository.GetStoredToDoItems();

        var updatedItem = storedItems.Find(i => i.ToDoItemId == existingId);

        Assert.IsType<NoContentResult>(result);
        Assert.NotNull(updatedItem);
        Assert.Equal(updatedName, updatedItem.Name);
        Assert.Equal(updatedDescription, updatedItem.Description);
        Assert.Equal(updatedIsCompleted, updatedItem.IsCompleted);
    }

    [Fact]
    public void UpdateById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var context = new ToDoItemsContext("DataSource=../../../IntegrationTests/data/localdb_test.db");
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);
        TestDataHelper.ClearTestData(repository);
        TestDataHelper.SeedTestData(repository);

        var nonExistingId = repository.GetStoredToDoItemsId().Max() + 1;
        var toDoItemDto = new ToDoItemUpdateRequestDto("Updated Item", "This item was updated", true, 2);

        // Act
        var result = controller.UpdateById(nonExistingId, toDoItemDto);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
}

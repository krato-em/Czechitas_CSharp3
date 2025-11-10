using System;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using ToDoList.Persistence;

namespace ToDoList.Test;

[Collection("Sequential")]
public class UpdateTests
{
    [Fact]
    public void UpdateById_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var context = new ToDoItemsContext("DataSource=../../../IntegrationTests/data/localdb_test.db");
        var controller = new ToDoItemsController(context);
        TestDataHelper.ClearTestData(controller);
        TestDataHelper.SeedTestData(controller);

        var existingToDoItemsId = controller.GetStoredToDoItemsId();
        var existingId = existingToDoItemsId.First();

        var updatedName = "Updated Item";
        var updatedDescription = "This item was updated";
        bool updatedIsCompleted = true;
        var toDoItemDto = new ToDoItemUpdateRequestDto(updatedName, updatedDescription, updatedIsCompleted);

        // Act
        var result = controller.UpdateById(existingId, toDoItemDto);

        // Assert
        // NOTE: myslenka za tim, proc ulozene ToDoItems ziskavam takto je ta, ze se chci vyhnout pouziti jinych HTTP metod, ktere testuji jinde
        var storedItems = controller.GetStoredToDoItems();

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
        var controller = new ToDoItemsController(context);
        TestDataHelper.ClearTestData(controller);
        TestDataHelper.SeedTestData(controller);

        var nonExistingId = controller.GetStoredToDoItemsId().Max() + 1;
        var toDoItemDto = new ToDoItemUpdateRequestDto("Updated Item", "This item was updated", true);

        // Act
        var result = controller.UpdateById(nonExistingId, toDoItemDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}

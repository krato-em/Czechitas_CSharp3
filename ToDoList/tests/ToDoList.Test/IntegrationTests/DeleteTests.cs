namespace ToDoList.Test;

using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using ToDoList.Persistence;

[Collection("Sequential")]
public class DeleteTests
{
    [Fact]
    public void DeleteByid_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var context = new ToDoItemsContext("DataSource=../../../IntegrationTests/data/localdb_test.db");
        var controller = new ToDoItemsController(context);
        TestDataHelper.ClearTestData(controller);
        TestDataHelper.SeedTestData(controller);
        var existingId = controller.GetStoredToDoItems().First().ToDoItemId;

        // Act
        var result = controller.DeleteByid(existingId);

        // Assert
        var actualItems = controller.GetStoredToDoItems();
        Assert.IsType<NoContentResult>(result);
        Assert.DoesNotContain(actualItems, item => item.ToDoItemId == existingId);
    }

    [Fact]
    public void DeleteById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var context = new ToDoItemsContext("DataSource=../../../IntegrationTests/data/localdb_test.db");
        var controller = new ToDoItemsController(context);
        TestDataHelper.ClearTestData(controller);
        TestDataHelper.SeedTestData(controller);

        // Act
        var invalidId = controller.GetStoredToDoItemsId().Last() +2;
        // invalidId = invalidId[0];
        // var invalidId = 1;
        var result = controller.DeleteByid(invalidId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}

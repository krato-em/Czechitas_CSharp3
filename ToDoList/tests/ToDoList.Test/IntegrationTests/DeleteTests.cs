namespace ToDoList.Test;

using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

[Collection("Sequential")]
public class DeleteTests
{
    [Fact]
    public void DeleteByid_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var controller = new ToDoItemsController();
        TestDataHelper.ClearTestData(controller);
        TestDataHelper.SeedTestData(controller);

        // Act
        var result = controller.DeleteByid(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.DoesNotContain(ToDoItemsController.items, item => item.ToDoItemId == 1);
    }

    [Fact]
    public void DeleteById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var controller = new ToDoItemsController();
        TestDataHelper.ClearTestData(controller);
        TestDataHelper.SeedTestData(controller);

        // Act
        var invalidId = 15;
        var result = controller.DeleteByid(invalidId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}

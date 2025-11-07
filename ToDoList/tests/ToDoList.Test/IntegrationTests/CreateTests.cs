namespace ToDoList.Test;

using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.WebApi;

[Collection("Sequential")]
public class CreateTests
{
    [Fact]
    public void Create_WithValidData_ReturnsCreatedResult()
{
        var context = new ToDoItemsContext("Data Source=../../../IntegrationTests/data/localdb_test.db");

        // Arrange
        var controller = new ToDoItemsController(context);
        TestDataHelper.ClearTestData(controller);

        // Act
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

        // Assert.Single(ToDoItemsController.items);
        // Assert.Equal(1, ToDoItemsController.items[0].ToDoItemId);
        Assert.Equal(1, context.ToDoItems.First().ToDoItemId);
    }
}

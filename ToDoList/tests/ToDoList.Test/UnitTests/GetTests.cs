using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

namespace ToDoList.Test.UnitTests;

public class GetUnitTests
{
    [Fact]
    public void Get_ReadWhenSomeItemAvailable_ReturnsOk()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var someItem = new ToDoItem { Name = "Test Name", Description = "testDescription", IsCompleted = false };

        var someItemList = new List<ToDoItem> { someItem };
        repositoryMock.ReadAllAsync().Returns(someItemList);
        // Faster way ho to write this
        // repositoryMock.ReadAllAsync().Returns([someItem]);

        // Act
        var result = controller.Read().Result;

        // Assert
        Assert.IsType<ActionResult<IEnumerable<ToDoItemGetResponseDto>>>(result);
        repositoryMock.Received(1).ReadAllAsync(); // tady kontroluju, ze se ta metoda opravdu zavolala; jako parametr uvadim, kolikrat cekam, ze se ta metoda zavola
    }

    [Fact]
    public void Get_ReadWhenNoItemAvailable_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var someEmptyList = new List<ToDoItem>();
        repositoryMock.ReadAllAsync().Returns(someEmptyList);

        // Act
        var result = controller.Read().Result;

        // Assert
        repositoryMock.Received(1).ReadAllAsync();
        Assert.Null(result.Value);
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void Get_ReadUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        repositoryMock.When(r => r.ReadAllAsync()).Do(r => throw new Exception());

        // Act
        var result = controller.Read().Result;

        // Assert
        Assert.Null(result.Value);
        repositoryMock.Received(1).ReadAllAsync();
        Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result).StatusCode);
    }
}

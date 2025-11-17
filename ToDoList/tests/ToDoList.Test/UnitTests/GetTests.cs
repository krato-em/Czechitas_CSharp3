using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.ExceptionExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ToDoList.Test.UnitTests;

public class GetUnitTests
{
    [Fact]
    public void Get_ReadWhenSomeItemAvailable_ReturnsOk()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var someItem = new ToDoItem { Name = "Test Name", Description = "testDescription", IsCompleted = false };

        var someItemList = new List<ToDoItem> { someItem };
        repositoryMock.ReadAll().Returns(someItemList);
        // Faster way ho to write this
        // repositoryMock.ReadAll().Returns([someItem]);

        // Act
        var result = controller.Read();

        // Assert
        Assert.IsType<ActionResult<IEnumerable<ToDoItemGetResponseDto>>>(result);
        repositoryMock.Received(1).ReadAll(); // tady kontroluju, ze se ta metoda opravdu zavolala; jako parametr uvadim, kolikrat cekam, ze se ta metoda zavola
    }
    [Fact]
    public void Get_ReadWhenNoItemAvailable_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var someEmptyList = new List<ToDoItem>();
        repositoryMock.ReadAll().Returns(someEmptyList);

        // Act
        var result = controller.Read();

        // Assert
        repositoryMock.Received(1).ReadAll();
        Assert.Null(result.Value);
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void Get_ReadUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        repositoryMock.When(r => r.ReadAll()).Do(r => throw new Exception());

        // Act
        var result = controller.Read();

        // Assert
        Assert.Null(result.Value);
        repositoryMock.Received(1).ReadAll();
        Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result).StatusCode);

    }
}

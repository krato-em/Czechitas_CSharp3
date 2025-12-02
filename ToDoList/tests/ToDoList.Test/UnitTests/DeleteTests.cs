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
using NSubstitute.Core.Arguments;

namespace ToDoList.Test.UnitTests;

public class DeleteTests
{
    [Fact]
    public async Task Delete_ValidItemId_ReturnsNoContent()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadByIdAsync(Arg.Any<int>()).Returns(
            new ToDoItem { Name = "Test Name", Description = "testDescription", IsCompleted = false }
        );
        var id = 1;

        // Act
        var result = controller.DeleteByid(id).Result;

        // Assert
        Assert.IsType<NoContentResult>(result);
        repositoryMock.Received(1).ReadByIdAsync(id);
        repositoryMock.Received(1).DeleteByIdAsync(id);
    }
    [Fact]
    public void Delete_InvalidItemId_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadByIdAsync(Arg.Any<int>()).Returns(null as ToDoItem);
        var someId = 1;

        // Act
        var result = controller.DeleteByid(someId).Result;

        // Assert
        Assert.IsType<NotFoundResult>(result);
        repositoryMock.Received(1).ReadByIdAsync(someId);
        repositoryMock.Received(0).DeleteByIdAsync(Arg.Any<int>()); // nothing was deleted
    }
    [Fact]
    public void Delete_AnyItemIdExceptionOccurredDuringReadById_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadByIdAsync(Arg.Any<int>()).Throws(new Exception());
        var someId = 1;

        // Act
        var result = controller.DeleteByid(someId).Result;

        // Assert
        Assert.IsType<ObjectResult>(result);
        repositoryMock.Received(1).ReadByIdAsync(someId);
        Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
    }
    [Fact]
    public void Delete_AnyItemIdExceptionOccurredDuringDeleteById_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadByIdAsync(Arg.Any<int>()).Returns(new ToDoItem { Name = "testItem", Description = "testDescription", IsCompleted = false });
        repositoryMock.When(r => r.DeleteByIdAsync(Arg.Any<int>())).Do(r => throw new Exception());
        var someId = 1;

        // Act
        var result = controller.DeleteByid(someId).Result;

        // Assert
        Assert.IsType<ObjectResult>(result);
        repositoryMock.Received(1).ReadByIdAsync(someId);
        repositoryMock.Received(1).DeleteByIdAsync(someId);
        Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
    }
}

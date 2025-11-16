using System;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;


namespace ToDoList.Test.UnitTests;

public class DeleteTests
{
    [Fact]
    public void Delete_ValidItemId_ReturnsNoContent()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadById(Arg.Any<int>()).Returns(
            new ToDoItem { Name = "Test Name", Description = "testDescription", IsCompleted = false }
        );
        var id = 1;

        // Act
        var result = controller.DeleteByid(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        repositoryMock.Received(1).ReadById(id);
        repositoryMock.Received(1).DeleteById(id);
    }
    [Fact]
    public void Delete_InvalidItemId_ReturnsNotFound()
    {
        // Arrange


        // Act


        // Assert
    }
    [Fact]
    public void Delete_AnyItemIdExceptionOccurredDuringReadById_ReturnsInternalServerError()
    {
        // Arrange


        // Act


        // Assert
    }
    [Fact]
    public void Delete_AnyItemIdExceptionOccurredDuringDeleteById_ReturnsInternalServerError()
    {
        // Arrange


        // Act


        // Assert
    }
}

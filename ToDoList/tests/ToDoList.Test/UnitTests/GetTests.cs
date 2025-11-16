using System;
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
    public void Get_ReadWhenSomeItemAvailable_ReturnsOk_try2()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        // Act


        // Assert
    }
}

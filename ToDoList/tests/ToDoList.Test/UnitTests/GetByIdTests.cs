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


namespace ToDoList.Test.UnitTests
{
    public class GetByIdTests
    {
        [Fact]
        public void Get_ReadByIdWhenSomeItemAvailable_ReturnsOk()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            var someItem = new ToDoItem { Name = "Test Name", Description = "testDescription", IsCompleted = false };
            var someId = 1;
            repositoryMock.ReadById(someId).Returns(someItem);

            // Act
            var result = controller.ReadById(someId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            repositoryMock.Received(1).ReadById(someId);
            Assert.Equal(someItem.Name, result.Value.Name);
            Assert.Equal(someItem.Description, result.Value.Description);
            Assert.Equal(someItem.IsCompleted, result.Value.IsCompleted);
        }


        [Fact]
        public void Get_ReadByIdWhenItemIsNull_ReturnsNotFound()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            var someId = 1;
            repositoryMock.ReadById(someId).Returns(null as ToDoItem);

            // Act
            var result = controller.ReadById(someId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result.Result);
            repositoryMock.Received(1).ReadById(someId);
        }

        [Fact]
        public void Get_ReadByIdUnhandledException_ReturnsInternalServerError()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            var someId = 1;
            repositoryMock.When(r => r.ReadById(Arg.Any<int>())).Do(r => throw new Exception());

            // Act
            var result = controller.ReadById(someId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result).StatusCode);
            repositoryMock.Received(1).ReadById(someId);
        }
    }
}

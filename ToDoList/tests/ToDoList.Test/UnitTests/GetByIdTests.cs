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
        public void Get_ReadByIdAsyncWhenSomeItemAvailable_ReturnsOk()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            var someItem = new ToDoItem { Name = "Test Name", Description = "testDescription", IsCompleted = false };
            var someId = 1;
            repositoryMock.ReadByIdAsync(someId).Returns(someItem);

            // Act
            var result = controller.ReadById(someId).Result;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            repositoryMock.Received(1).ReadByIdAsync(someId);
        }


        [Fact]
        public void Get_ReadByIdAsyncWhenItemIsNull_ReturnsNotFound()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            var someId = 1;
            repositoryMock.ReadByIdAsync(someId).Returns(null as ToDoItem);

            // Act
            var result = controller.ReadById(someId).Result;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result.Result);
            repositoryMock.Received(1).ReadByIdAsync(someId);
        }

        [Fact]
        public void Get_ReadByIdAsyncUnhandledException_ReturnsInternalServerError()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            var someId = 1;
            repositoryMock.When(r => r.ReadByIdAsync(Arg.Any<int>())).Do(r => throw new Exception());

            // Act
            var result = controller.ReadById(someId).Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result).StatusCode);
            repositoryMock.Received(1).ReadByIdAsync(someId);
        }
    }
}

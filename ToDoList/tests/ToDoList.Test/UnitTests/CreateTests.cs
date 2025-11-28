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


namespace ToDoList.Test.UnitTests
{
    public class CreateTests
    {
        [Fact]
        public void Post_CreateValidRequest_ReturnsCreatedAtAction()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);

            var itemToCreate = new ToDoItemCreateRequestDto("Test Name", "Create Test", false);

            // Act
            var result = controller.Create(itemToCreate);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
            repositoryMock.Received(1).Create(Arg.Is<ToDoItem>(i =>
                i.Name == itemToCreate.Name &&
                i.Description == itemToCreate.Description
                && i.IsCompleted == itemToCreate.IsCompleted));
        }

        [Fact]
        public void Post_CreateUnhandledException_ReturnsInternalServerError()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            repositoryMock.When(r => r.Create(Arg.Any<ToDoItem>())).Do(r => throw new Exception());

            var itemToCreate = new ToDoItemCreateRequestDto("Test Name", "Create Test", false);

            // Act
            var result = controller.Create(itemToCreate);

            // Assert
            Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result).StatusCode);
            repositoryMock.Received(1).Create(Arg.Any<ToDoItem>());
        }
    }
}

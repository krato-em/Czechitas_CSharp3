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
    public class UpdateByIdTests
    {
        [Fact]
        public void Put_UpdateByIdWhenItemUpdated_ReturnsNoContent()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            repositoryMock.ReadById(Arg.Any<int>()).Returns(new ToDoItem { Name = "testItem", Description = "testDescription", IsCompleted = false });
            int someId = 1;

            var updatedItemDto = new ToDoItemUpdateRequestDto("UpdatedItem", "This item was updated", true);


            // Act
            var result = controller.UpdateById(someId, updatedItemDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            repositoryMock.Received(1).ReadById(someId);
            repositoryMock.Received(1).Update(Arg.Any<ToDoItem>());
            repositoryMock.Received(1).Update(Arg.Is<ToDoItem>(item =>
                item.ToDoItemId == someId &&
                item.Name == updatedItemDto.Name &&
                item.Description == updatedItemDto.Description &&
                item.IsCompleted == updatedItemDto.IsCompleted
            ));
        }
        [Fact]
        public void Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            repositoryMock.ReadById(Arg.Any<int>()).Returns(null as ToDoItem);
            int someId = 1;
            var updatedItemDto = new ToDoItemUpdateRequestDto("UpdatedItem", "This item was updated", true);

            // Act
            var result = controller.UpdateById(someId, updatedItemDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            repositoryMock.Received(1).ReadById(someId);
            repositoryMock.Received(0).Update(Arg.Any<ToDoItem>());
        }
        [Fact]
        public void Put_UpdateByIdUnhandledException_ReturnsInternalServerError()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);
            repositoryMock.ReadById(Arg.Any<int>()).Returns(new ToDoItem { Name = "testItem", Description = "testDescription", IsCompleted = false });
            repositoryMock.When(r => r.Update(Arg.Any<ToDoItem>())).Do(r => throw new Exception());
            int someId = 1;
            var updatedItemDto = new ToDoItemUpdateRequestDto("UpdatedItem", "This item was updated", true);

            // Act
            var result = controller.UpdateById(someId, updatedItemDto);
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
            repositoryMock.Received(1).ReadById(someId);
            repositoryMock.Received(1).Update(Arg.Any<ToDoItem>());
        }
    }
}

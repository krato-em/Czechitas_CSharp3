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
    public class UpdateByIdTests
    {
        [Fact]
        public void Put_UpdateByIdWhenItemUpdated_ReturnsNoContent()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(repositoryMock);

            var someItem = new ToDoItem { Name = "Test Name", Description = "testDescription", IsCompleted = false };
        }

        [Fact]
        public void Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
        {

        }
        [Fact]
        public void Put_UpdateByIdUnhandledException_ReturnsInternalServerError()
        {

        }
    }
}

namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.WebApi;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

[Collection("Sequential")]
public class DeleteTests
{
    [Fact]
    public void DeleteByid_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var context = new ToDoItemsContext("DataSource=../../../IntegrationTests/data/localdb_test.db");
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);
        TestDataHelper.ClearTestData(repository);
        TestDataHelper.SeedTestData(repository);
        var existingId = repository.GetStoredToDoItems().First().ToDoItemId;

        // Act
        var result = controller.DeleteByid(existingId);

        // Assert
        var actualItems = repository.GetStoredToDoItems();
        Assert.IsType<NoContentResult>(result);
        Assert.DoesNotContain(actualItems, item => item.ToDoItemId == existingId);
    }

    [Fact]
    public void DeleteById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var context = new ToDoItemsContext("DataSource=../../../IntegrationTests/data/localdb_test.db");
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);
        TestDataHelper.ClearTestData(repository);
        TestDataHelper.SeedTestData(repository);

        // Act
        var invalidId = repository.GetStoredToDoItemsId().Last() + 2;
        var result = controller.DeleteByid(invalidId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}

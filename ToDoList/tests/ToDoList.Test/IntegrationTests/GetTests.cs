namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.WebApi;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

[Collection("Sequential")]
public class GetTests
{
    [Fact]
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        var context = new ToDoItemsContext("DataSource=../../../IntegrationTests/data/localdb_test.db");
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);
        TestDataHelper.ClearTestData(repository);
        TestDataHelper.SeedTestData(repository);

        // Act

        var result = controller.Read();
        var value = ActionResultExtensions.GetValue(result);

        // Assert
        Assert.NotNull(value);
        var firstToDo = value.First();
        Assert.Equal(TestDataHelper.toDoItem1.ToDoItemId, firstToDo.Id);
        Assert.Equal(TestDataHelper.toDoItem1.Name, firstToDo.Name);
        Assert.Equal(TestDataHelper.toDoItem1.Description, firstToDo.Description);
        Assert.Equal(TestDataHelper.toDoItem1.IsCompleted, firstToDo.IsCompleted);
    }

    [Fact]
    public void Get_ItemById_ReturnItem()
    {
        // Arrange
        var context = new ToDoItemsContext("DataSource=../../../IntegrationTests/data/localdb_test.db");
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);
        TestDataHelper.ClearTestData(repository);
        TestDataHelper.SeedTestData(repository);

        // Act

        //TODO: toto musim fixnout, aby to bralo ID z databaze
        var id = 2;
        var result = controller.ReadById(id);
        var value = ActionResultExtensions.GetValue(result);

        // Assert
        Assert.Equal(id, value.Id);
        Assert.Equal(TestDataHelper.toDoItem2.Name, value.Name);
        Assert.Equal(TestDataHelper.toDoItem2.Description, value.Description);
        Assert.Equal(TestDataHelper.toDoItem2.IsCompleted, value.IsCompleted);
    }

    [Fact]
    public void Get_ItemByInvaliId_ReturnNotFound()
    {
        // Arrange
        var context = new ToDoItemsContext("DataSource=../../../IntegrationTests/data/localdb_test.db");
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);
        TestDataHelper.ClearTestData(repository);
        TestDataHelper.SeedTestData(repository);

        // Act
        var invalidId = repository.GetStoredToDoItemsId().Last() + 2;
        var result = controller.ReadById(invalidId);
        var actualItems = repository.GetStoredToDoItems();

        // Assert
        Assert.NotNull(result.Result);
        Assert.IsType<NotFoundResult>(result.Result);
        Assert.DoesNotContain(actualItems, item => item.ToDoItemId == invalidId);
    }
}

namespace ToDoList.Test;

using ToDoList.Domain.DTOs;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

[Collection("Sequential")]
public class CreateTests
{
    [Fact]
    public void Create_WithValidData_ReturnsCreatedResult()
    {
        // Arrange
        var context = new ToDoItemsContext("DataSource=../../../IntegrationTests/data/localdb_test.db");
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);
        TestDataHelper.ClearTestData(repository);

        // Act
        var request = new ToDoItemCreateRequestDto("addImte", "addDesc", true);

        var actionResult = controller.Create(request);
        var result = actionResult.Result;
        var dto = actionResult.GetValue();

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(dto);
        Assert.Equal("addImte", dto.Name);
        Assert.Equal("addDesc", dto.Description);
        Assert.Equal(request.IsCompleted, dto.IsCompleted);
        Assert.Single(repository.GetStoredToDoItems());
    }
}

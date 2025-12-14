namespace ToDoList.Frontend.Clients;

using ToDoList.Frontend.Views;

public interface IToDoItemsClient
{
    public Task<List<ToDoItemView>> ReadItemsAsync();
    public Task<ToDoItemView?> ReadItemByIdAsync(int itemId);

    public Task UpdateItemAsync(ToDoItemView item);
}

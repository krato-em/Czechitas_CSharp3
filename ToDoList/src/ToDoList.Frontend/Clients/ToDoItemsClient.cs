namespace ToDoList.Frontend.Clients;

using System.Net;
using ToDoList.Domain.DTOs;
using ToDoList.Frontend.Views;
public class ToDoItemsClient : IToDoItemsClient
{
    private readonly HttpClient httpClient;
    public ToDoItemsClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public async Task<List<ToDoItemView>> ReadItemsAsync()
    {
        var toDoItemViews = new List<ToDoItemView>();
        var response = await httpClient.GetFromJsonAsync<List<ToDoItemGetResponseDto>>("api/ToDoItems"); // v tenhle krok potrebujeme vylozene pockat na vysledek - nechceme jit dal dokud nemame odpoved -> proto 'await'
        // to 'await' zpusobuje i to, ze se nam nevraci Response, ale vraci se nam List<>

        toDoItemViews = response.Select(dto => new ToDoItemView()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted
        }).ToList();

        return toDoItemViews;
    }
    public async Task<ToDoItemView?> ReadItemByIdAsync(int itemId)
    {
        var response = await httpClient.GetFromJsonAsync<ToDoItemGetResponseDto>($"api/ToDoItems/{itemId}");

        var toDoItem = new ToDoItemView()
        {
            Id = response.Id,
            Name = response.Name,
            Description = response.Description,
            IsCompleted = response.IsCompleted
        };
        return toDoItem;
    }

    public async Task UpdateItemAsync(ToDoItemView item)
    {
        //TODO: zabalit to do Try/Catch
        var itemRequest = new ToDoItemUpdateRequestDto(item.Name, item.Description, item.IsCompleted);
        var response = await httpClient.PutAsJsonAsync($"api/ToDoItems/{item.Id:int}", itemRequest);
    }
}

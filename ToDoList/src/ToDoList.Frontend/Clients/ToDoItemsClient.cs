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

        toDoItemViews = response.Select(dto => new ToDoItemView(
            dto.Id,
            dto.Name,
            dto.Description,
            dto.IsCompleted
            )).ToList();

        return toDoItemViews;
    }
}

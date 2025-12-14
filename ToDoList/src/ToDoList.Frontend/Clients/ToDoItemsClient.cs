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

        try
        {
            Console.WriteLine($"=== Starting Update ===");
            Console.WriteLine($"Item ID: {item.Id}");
            Console.WriteLine($"Item Name: {item.Name}");
            Console.WriteLine($"Item IsCompleted: {item.IsCompleted}");

            var itemRequest = new ToDoItemUpdateRequestDto(item.Name, item.Description, item.IsCompleted);

            Console.WriteLine($"Sending PUT to: api/ToDoItems/{item.Id}");

            // var response = await httpClient.PutAsJsonAsync($"api/ToDoItems/{item.Id:int}", itemRequest);
            var response = await httpClient.PutAsJsonAsync($"api/ToDoItems/{item.Id}", itemRequest);

            Console.WriteLine($"Response Status Code: {response.StatusCode}");
            Console.WriteLine($"Response IsSuccess: {response.IsSuccessStatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Content: {errorContent}");
            }

            // response.EnsureSuccessStatusCode();
            Console.WriteLine("=== Update Successful ===");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception caught: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }

    public async Task DeleteItemAsync(ToDoItemView item)
    {
        await httpClient.DeleteAsync($"api/ToDoItems/{item.Id}");
    }
}

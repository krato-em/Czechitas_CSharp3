namespace ToDoList.WebApi;

using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;

[Route("api/[controller]")] //localhost:500/api/ToDoItems
[ApiController]
public class ToDoItemsController : ControllerBase
{

    public static List<ToDoItem> items = [];
    // ToDoItemCreateRequestDto createRequestDto = new ToDoItemCreateRequestDto();

    private readonly ToDoItemsContext context;

    public ToDoItemsController(ToDoItemsContext context)
    {
        this.context = context;

        // ToDoItem item = new ToDoItem
        // {
        //     Name = "Prvni ukol",
        //     Description = "Prvni popisek",
        //     IsCompleted = false
        // };

        // context.ToDoItems.Add(item);
        // context.SaveChanges();
    }

    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request)
    {
        var item = request.ToDomain();

        try
        {
            // item.ToDoItemId = items.Count == 0 ? 1 : items.Max(o => o.ToDoItemId) + 1;
            // items.Add(item);
            context.ToDoItems.Add(item);
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        //respond to client
        return CreatedAtAction(
            nameof(ReadById),
            new { toDoItemId = item.ToDoItemId },
            ToDoItemGetResponseDto.FromDomain(item)); //201
    }

    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemGetResponseDto>> Read()
    {
        List<ToDoItem> itemsToGet;
        try
        {
            // itemsToGet = items;
            itemsToGet = context.ToDoItems.AsNoTracking().ToList();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return (itemsToGet is null)
            ? NotFound() //404
            : Ok(itemsToGet.Select(ToDoItemGetResponseDto.FromDomain)); //200
    }

    [HttpGet("{todoItemId:int}")]
    public ActionResult<ToDoItemGetResponseDto> ReadById(int toDoItemId)
    {
        ToDoItem? itemToGet;
        try
        {
            itemToGet = items.Find(i => i.ToDoItemId == toDoItemId);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return (itemToGet is null)
            ? NotFound() //404
            : Ok(ToDoItemGetResponseDto.FromDomain(itemToGet)); //200
    }

    [HttpPut("{todoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        //map to Domain object as soon as possible
        var updatedItem = request.ToDomain();

        //try to update the item by retrieving it with given id
        try
        {
            //retrieve the item
            var itemIndexToUpdate = items.FindIndex(i => i.ToDoItemId == toDoItemId);
            if (itemIndexToUpdate == -1)
            {
                return NotFound(); //404
            }
            updatedItem.ToDoItemId = toDoItemId;
            items[itemIndexToUpdate] = updatedItem;
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return NoContent(); //204
    }

    [HttpDelete("{todoItemId:int}")]
    public IActionResult DeleteByid(int todoItemId)
    {
        //try to delete the item
        try
        {
            var itemToDelete = items.Find(i => i.ToDoItemId == todoItemId);
            if (itemToDelete is null)
            {
                return NotFound(); //404
            }
            items.Remove(itemToDelete);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        //respond to client
        return NoContent(); //204
    }

    public void AddItemToStorage(ToDoItem item)
    {
        items.Add(item);
    }

    public void ClearStorage()
    {
        items.Clear();
    }

    public List<ToDoItem> GetStoredToDoItems()
    {
        return items;
    }

    public List<int> GetStoredToDoItemsId()
    {
        return items.Select((item) => item.ToDoItemId).ToList();
    }
}

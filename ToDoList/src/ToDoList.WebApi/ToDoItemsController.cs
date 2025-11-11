namespace ToDoList.WebApi;

using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

[Route("api/[controller]")] //localhost:500/api/ToDoItems
[ApiController]
public class ToDoItemsController : ControllerBase
{

    // public static List<ToDoItem> items = [];
    // ToDoItemCreateRequestDto createRequestDto = new ToDoItemCreateRequestDto();

    private readonly ToDoItemsContext context;
    private readonly IRepository<ToDoItem> repository; // we should not create references for classes, but for interfaces it's ok for safety reasons
    public ToDoItemsController(ToDoItemsContext context, IRepository<ToDoItem> repository)
    {
        this.context = context;
        this.repository = repository;

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
            // old implementation before we stored data in a database
            // item.ToDoItemId = items.Count == 0 ? 1 : items.Max(o => o.ToDoItemId) + 1;
            // items.Add(item);

            // This part is moved to ToDoItemsRepository class so that we don't have a tight coupling and that this class doesn't directly communicate with the db layer
            // context.ToDoItems.Add(item);
            // context.SaveChanges();

            repository.Create(item);
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
        // List<ToDoItem> itemsToGet;
        IEnumerable<ToDoItem> itemsToGet;

        try
        {
            // itemsToGet = items;
            // itemsToGet = context.ToDoItems.AsNoTracking().ToList();
            itemsToGet = repository.ReadAll();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

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
            // itemToGet = items.Find(i => i.ToDoItemId == toDoItemId);
            // itemToGet = context.ToDoItems.Single(item => item.ToDoItemId == toDoItemId);
            // itemToGet = context.ToDoItems.AsNoTracking().Single(item => item.ToDoItemId == toDoItemId);
            itemToGet = repository.ReadById(toDoItemId);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        return (itemToGet is null)
            ? NotFound() //404
            : Ok(ToDoItemGetResponseDto.FromDomain(itemToGet)); //200
    }

    [HttpPut("{todoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        //map to Domain object as soon as possible
        var updatedItem = request.ToDomain();
        updatedItem.ToDoItemId = toDoItemId;

        //try to update the item by retrieving it with given id
        try
        {
            //retrieve the item

            // var itemIndexToUpdate = items.FindIndex(i => i.ToDoItemId == toDoItemId);
            // if (itemIndexToUpdate == -1)
            // {
            //     return NotFound(); //404
            // }
            // updatedItem.ToDoItemId = toDoItemId;
            // items[itemIndexToUpdate] = updatedItem;

            // var itemToUpdate = context.ToDoItems.Find(toDoItemId);
            var itemToUpdate = repository.ReadById(toDoItemId);
            if (itemToUpdate == null)
            {
                return NotFound();
            }

            itemToUpdate.Name = updatedItem.Name;
            itemToUpdate.Description = updatedItem.Description;
            itemToUpdate.IsCompleted = updatedItem.IsCompleted;

            // context.SaveChanges();
            repository.Update(updatedItem);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        return NoContent(); //204
    }

    [HttpDelete("{todoItemId:int}")]
    public IActionResult DeleteByid(int todoItemId)
    {
        try
        {

            var itemToDelete = repository.ReadById(todoItemId);
            if (itemToDelete is null)
            {
                return NotFound(); //404
            }

            repository.DeleteById(todoItemId);

            // // Option 2 - Found in EF documentation. Shorter code, single line. But I'm not throwing NotFound if the item to be deleted doesn't exist
            // context.Remove(context.ToDoItems.Single(x => x.ToDoItemId == todoItemId));
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        return NoContent(); //204
    }

    public void AddItemToStorage(ToDoItem item)
    {
        // items.Add(item);
        // context.Add(item);
        context.ToDoItems.Add(item);
        context.SaveChanges();
    }
    public void ClearStorage()
    {
        // items.Clear();
        // context.Database.ExecuteSqlRaw("TRUNCATE TABLE [ToDoItems]");
        // var rows = from o in context.ToDoItems select o;
        // foreach (var row in rows)
        // {
        //     context.ToDoItems.Remove(row);
        // }
        context.ToDoItems.ExecuteDelete();
        context.SaveChanges();
    }

    public List<ToDoItem> GetStoredToDoItems()
    {
        var data = context.ToDoItems.ToList();
        return data;
    }

    public List<int> GetStoredToDoItemsId()
    {
        var data = context.ToDoItems.Select(x => x.ToDoItemId).ToList();
        return data;
    }
}

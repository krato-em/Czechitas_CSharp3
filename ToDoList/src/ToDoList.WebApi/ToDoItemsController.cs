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
    private readonly IRepository<ToDoItem> repository; // we should not create references for classes, but for interfaces it's ok for safety reasons
    public ToDoItemsController(IRepository<ToDoItem> repository)
    {
        this.repository = repository;
    }

    [HttpPost]
    public ActionResult<ToDoItemGetResponseDto> Create(ToDoItemCreateRequestDto request)
    {
        var item = request.ToDomain();

        try
        {
            // // This part is moved to ToDoItemsRepository class so that we don't have a tight coupling and that this class doesn't directly communicate with the db layer
            // context.ToDoItems.Add(item);
            // context.SaveChanges();

            repository.Create(item);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        return CreatedAtAction(
            nameof(ReadById),
            new { toDoItemId = item.ToDoItemId },
            ToDoItemGetResponseDto.FromDomain(item)); //201
    }

    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemGetResponseDto>> Read()
    {
        IEnumerable<ToDoItem> itemsToGet;

        try
        {
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
    public ActionResult  UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        var updatedItem = request.ToDomain();
        updatedItem.ToDoItemId = toDoItemId;

        try
        {
            var itemToUpdate = repository.ReadById(toDoItemId);
            if (itemToUpdate == null)
            {
                return NotFound();
            }

            itemToUpdate.Name = updatedItem.Name;
            itemToUpdate.Description = updatedItem.Description;
            itemToUpdate.IsCompleted = updatedItem.IsCompleted;

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
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        return NoContent(); //204
    }
}

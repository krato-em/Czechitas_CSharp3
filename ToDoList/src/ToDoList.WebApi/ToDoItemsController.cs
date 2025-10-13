namespace ToDoList.WebApi;

using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[Route("api/[controller]")] //localhost:500/api/ToDoItems
[ApiController]
public class ToDoItemsController : ControllerBase
{

    private static List<ToDoItem> items = [];

    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request)
    {
        return Ok();
    }

    [HttpGet]
    public IActionResult Read()
    {
        return Ok();

    }

    [HttpGet("{todoItemId:int}")]
    public IActionResult ReadById(int todoItemId) //api/ToDoItems/<id> GET
    {
        try
        {
            throw new Exception("Neco se pokazilo");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        return Ok();

    }

    [HttpPut("{todoItemId:int}")]
    public IActionResult UpdateById(int todoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        return Ok();
    }

    [HttpDelete("{todoItemId:int}")]
    public IActionResult DeleteByid(int todoItemId)
    {
        return Ok();

    }
}

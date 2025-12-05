using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

namespace ToDoList.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IRepositoryAsync<Category> repository;
        public CategoriesController(IRepositoryAsync<Category> repository)
        {
            this.repository = repository;
        }
        [HttpPost]
        public async Task<ActionResult<CategoryGetResponseDto>> Create(CategoryCreateRequestDto request)
        {
            var item = request.ToDomain();
            try
            {
                await repository.CreateAsync(item);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction(
                nameof(ReadById),
                new { categoryId = item.CategoryId },
                CategoryGetResponseDto.FromDomain(item));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryGetResponseDto>>> Read()
        {
            IEnumerable<Category> categoriesToGet;

            try
            {
                categoriesToGet = await repository.ReadAllAsync();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
            }

            return (categoriesToGet is null || !categoriesToGet.Any())
                ? NotFound()
                : Ok(categoriesToGet.Select(CategoryGetResponseDto.FromDomain));
        }

        [HttpGet("{categoryId:int}")]
        public async Task<ActionResult<CategoryGetResponseDto>> ReadById(int categoryId)
        {
            Category? itemToGet;
            try
            {
                itemToGet = await repository.ReadByIdAsync(categoryId);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
            }

            return (itemToGet is null)
                ? NotFound()
                : Ok(CategoryGetResponseDto.FromDomain(itemToGet));
        }

        [HttpPut("{categoryId:int}")]
        public async Task<ActionResult> UpdateById(int categoryId, [FromBody] CategoryUpdateRequestDto request)
        {
            var updatedItem = request.ToDomain();
            updatedItem.CategoryId = categoryId;

            try
            {
                var itemToUpdate = await repository.ReadByIdAsync(categoryId);
                if (itemToUpdate == null)
                {
                    return NotFound();
                }

                await repository.UpdateAsync(updatedItem);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
            }

            return NoContent(); //204
        }

        [HttpDelete("{categoryId:int}")]
        public async Task<IActionResult> DeleteByid(int categoryId)
        {
            try
            {
                var itemToDelete = await repository.ReadByIdAsync(categoryId);
                if (itemToDelete is null)
                {
                    return NotFound(); //404
                }

                await repository.DeleteByIdAsync(categoryId);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
            }

            return NoContent(); //204
        }
    }
}

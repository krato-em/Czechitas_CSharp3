using System;
using Microsoft.CodeAnalysis.Host;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

namespace ToDoList.Persistence.Repositories;

public class CategoriesRepository : IRepositoryAsync<Category>
{
    private readonly ToDoItemsContext context;
    public CategoriesRepository(ToDoItemsContext context)
    {
        this.context = context;
    }

    public async Task CreateAsync(Category category)
    {
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
    }
    public async Task<IEnumerable<Category>> ReadAllAsync()
    {
        return await context.Categories.ToListAsync();
    }
    public async Task<Category?> ReadByIdAsync(int id)
    {
        return await context.Categories.FindAsync(id);
    }

    public async Task UpdateAsync(Category category)
    {
        var itemToUpdate = await context.Categories.FindAsync(category.CategoryId) ?? throw new ArgumentOutOfRangeException($"Category with ID {category.CategoryId} not found.");
        context.Entry(itemToUpdate).CurrentValues.SetValues(category);
        await context.SaveChangesAsync();
    }
    public async Task DeleteByIdAsync(int id)
    {
        var itemToDelete = await context.Categories.FindAsync(id) ?? throw new ArgumentOutOfRangeException($"Category with ID {id} not found.");
        context.Categories.Remove(itemToDelete);
        await context.SaveChangesAsync();
    }

    public void AddCategoryToStorage(Category item)
    {
        context.Categories.Add(item);
        context.SaveChanges();
    }

    public void ClearStorage()
    {
        context.Categories.ExecuteDelete();
        context.SaveChanges();
    }

    public List<Category> GetStoredCategories()
    {
        var data = context.Categories.ToList();
        return data;
    }
    public List<int> GetStoredCategoryIds()
    {
        var data = context.Categories.Select(x => x.CategoryId).ToList();
        return data;
    }
}

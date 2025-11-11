namespace ToDoList.Persistence.Repositories
{
    using System.Threading.Tasks;
    using ToDoList.Domain.Models;
    using System.Collections.Generic;

    public class ToDoItemsRepository : IRepository<ToDoItem>
    {
        private readonly ToDoItemsContext context;
        public ToDoItemsRepository(ToDoItemsContext context)
        {
            this.context = context;
        }
        public void Create(ToDoItem item)
        {
            context.ToDoItems.Add(item);
            context.SaveChanges();
        }
        public IEnumerable<ToDoItem> ReadAll() => context.ToDoItems.ToList();
        public ToDoItem? ReadById(int id) => context.ToDoItems.Find(id);
        public void Update(ToDoItem item)
        {
            var foundItem = context.ToDoItems.Find(item.ToDoItemId) ?? throw new ArgumentOutOfRangeException($"ToDo item with ID {item.ToDoItemId} not found.");
            context.Entry(foundItem).CurrentValues.SetValues(item);
            context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var item = context.ToDoItems.Find(id) ?? throw new ArgumentOutOfRangeException($"ToDo item with ID {id} not found.");
            context.ToDoItems.Remove(item);
            context.SaveChanges();
        }
    }
}

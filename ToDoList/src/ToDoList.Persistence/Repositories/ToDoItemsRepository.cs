namespace ToDoList.Persistence.Repositories
{
    using System.Threading.Tasks;
    using ToDoList.Domain.Models;

    public class ToDoItemsRepository : IRepository<ToDoItem>
    {
        
        public void Create(Task item) => throw new NotImplementedException();
    }
}

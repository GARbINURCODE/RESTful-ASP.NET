using Microsoft.EntityFrameworkCore;
using RESTfull.Domain.Models;
using RESTfull.Infrastructure.Data;

namespace RESTfull.Infrastructure.Repositories 
{
    public class PersonRepository : BaseRepository<Person> // Репозиторий для класса Person
    {
        public Context UnitOfWork // Метод для получения текущего Context`а
        {
            get { return Context; }
        }

        public PersonRepository(Context context) : base(context) { } // Конструктор с параметром объекта Context`а

        public async new Task<List<Person>> GetAllAsync()
        {
            return await Context.Persons
                .OrderBy(p => p.Name)
                .Include(p => p.Orders)
                .ToListAsync();
        }

        public async new Task<Person?> GetByIdAsync(Guid id) // Метод для получения персоны по Id 
        {
            return await Context.Persons
                .Where(prop => prop.Id == id)
                .Include(p => p.Orders)
                .FirstOrDefaultAsync();
        }

        public async Task<Person?> GetByNameAsync(string name) // Метод для получения персоны по имени
        {
            return await Context.Persons
                .Where(prop => prop.Name == name)
                .Include(p => p.Orders)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(Guid id) // Метод для удаления персоны по Id
        {
            Person? person = await GetByIdAsync(id);
            if (person != null)
            {
                foreach(Product product in person.Orders)
                {
                    Context.Set<Product>().Remove(product);
                }
                Context.Persons.Remove(person);
                await Context.SaveChangesAsync();
            }
        }

        public void ChangeTrackerClear() { Context.ChangeTracker.Clear(); }

        public async new Task UpdateAsync(Person person) // Асинхронный метод для обновления данных о персоне
        {
            var existPerson = await Context.Persons.FindAsync(person.Id);
            if (existPerson != null)
            {
                Context.Entry(existPerson).CurrentValues.SetValues(person);
                foreach (var product in person.Orders)
                {
                    var existProduct =
                        existPerson.Orders.FirstOrDefault(pr => pr.Id == product.Id);
                    if (existProduct == null)
                    {
                        existPerson.Orders.Add(product);
                        await Context.Set<Product>().AddAsync(product);
                    }
                    else
                    {
                        Context.Entry(existProduct).CurrentValues.SetValues(product);
                    }
                }
                foreach (var existProduct in existPerson.Orders)
                {
                    if (!person.Orders.Any(pr => pr.Id == existProduct.Id))
                    {
                        Context.Remove(existProduct);
                    }
                }
            }
            await Context.SaveChangesAsync();
        }

    }
}

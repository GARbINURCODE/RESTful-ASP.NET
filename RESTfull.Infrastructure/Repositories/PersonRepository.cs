using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using RESTfull.Domain.Models;
using RESTfull.Infrastructure.Data;

namespace RESTfull.Infrastructure.Repositories 
{
    public class PersonRepository : BaseRepository<Person> // Репозиторий для класса Person
    {
        private readonly Context _context; // Поле используемого класса Context
        public Context UnitOfWork // Метод для получения текущего Context`а
        {
            get { return _context; }
        }

        public PersonRepository(Context context) : base(context) // Конструктор с параметром объекта Context`а
        {
            // То, что в методе, это обработка исключений?
            _context = context;
        }

        public async new Task<List<Person>> GetAllAsync()
        {
            return await _context.Persons
                .OrderBy(p => p.Name)
                .Include(p => p.Orders)
                .ToListAsync();
        }

        public async new Task<Person?> GetByIdAsync(Guid id) // Метод для получения персоны по Id 
        {
            return await _context.Persons
                .Where(prop => prop.Id == id)
                .Include(p => p.Orders)
                .FirstOrDefaultAsync();
        }

        public async Task<Person?> GetByNameAsync(string name) // Метод для получения персоны по имени
        {
            return await _context.Persons
                .Where(prop => prop.Name == name)
                .Include(p => p.Orders)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(Guid id) // Метод для удаления персоны по Id
        {
            Person? person = await _context.Persons.FindAsync(id);
            if (person != null)
            {
                _context.Remove(person);
                await _context.SaveChangesAsync();
            }
        }

        public void ChangeTrackerClear() { _context.ChangeTracker.Clear(); }

        public async new Task UpdateAsync(Person person) // Асинхронный метод для обновления данных о персоне
        {
            var existPerson = await _context.Persons.FindAsync(person.Id);
            if (existPerson != null)
            {
                _context.Entry(existPerson).CurrentValues.SetValues(person);
                foreach (var product in person.Orders)
                {
                    var existProduct =
                        existPerson.Orders.FirstOrDefault(pr => pr.Id == product.Id);
                    if (existProduct == null)
                    {
                        existPerson.Orders.Add(product);
                    }
                    else
                    {
                        _context.Entry(existProduct).CurrentValues.SetValues(product);
                    }
                }
                foreach (var existProduct in existPerson.Orders)
                {
                    if (!person.Orders.Any(pr => pr.Id == existProduct.Id))
                    {
                        _context.Remove(existProduct);
                    }
                }
            }
            await _context.SaveChangesAsync();
        }

    }
}

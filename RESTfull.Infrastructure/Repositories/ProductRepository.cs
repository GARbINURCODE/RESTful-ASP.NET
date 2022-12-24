using System.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RESTfull.Domain.Models;
using RESTfull.Infrastructure.Data;

namespace RESTfull.Infrastructure.Repositories
{
    public class ProductRepository<TProductModel>  : BaseRepository<TProductModel> where TProductModel : Product
    {
        private PersonRepository PersonRepository { get; set; }
        public ProductRepository(Context context) : base(context)
        {
            PersonRepository = new PersonRepository(context);
        }

        public async new Task<List<TProductModel>> GetAllAsync()
        {
            return await Context.Set<TProductModel>()
                .Include(p => p.Person)
                .ToListAsync();
        }

        public async new Task<TProductModel?> GetByIdAsync(Guid id)
        {
            return await Context.Set<TProductModel>()
                .Where(p => p.Id == id)
                .Include(p => p.Person)
                .FirstOrDefaultAsync();
        }

        public async Task<List<TProductModel>> GetByPersonAsync(Person person)
        {
            return await Context.Set<TProductModel>()
                .Where(m => m.Person.Id == person.Id)
                .ToListAsync();
        }

        public async new Task<TProductModel?> CreateAsync(TProductModel product)
        {
            var person = await PersonRepository.GetByIdAsync(product.Person!.Id);
            if (person != null)
            {
                person.Orders.Add(product);
                await Context.Set<TProductModel>().AddAsync(product);
                await PersonRepository.UpdateAsync(person);
            }
            return product;
        }

        public async Task DeleteAsync(Guid id)
        {
            var victim = await GetByIdAsync(id);
            if (victim != null)
            {
                var person = await PersonRepository.GetByIdAsync(victim.Person!.Id);
                if (person != null)
                {
                    person.Orders.Remove(victim);
                    await PersonRepository.UpdateAsync(person);
                }
                Context.Set<TProductModel>().Remove(victim);
                await Context.SaveChangesAsync();
            }
        }

    }
}

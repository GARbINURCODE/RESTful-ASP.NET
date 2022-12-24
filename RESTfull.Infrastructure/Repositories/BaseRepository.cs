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
    public class BaseRepository<TDbModel> : IBaseRepository<TDbModel> where TDbModel : BaseModel
    {
        private protected Context Context { get; set; }

        public BaseRepository(Context context) { Context = context; }

        public async Task<List<TDbModel>> GetAllAsync()
        {
            return await Context.Set<TDbModel>().ToListAsync();
        }

        public async Task<TDbModel?> GetByIdAsync(Guid id)
        {
            return await Context.Set<TDbModel>()
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<TDbModel> CreateAsync(TDbModel model)
        {
            await Context.Set<TDbModel>().AddAsync(model);
            await Context.SaveChangesAsync();
            return model;
        }

        public async Task UpdateAsync(TDbModel model)
        {
            var suspect = await GetByIdAsync(model.Id);
            if (suspect != null)
            {
                Context.Update(suspect);
                await Context.SaveChangesAsync();
            }
            else { await CreateAsync(model); }
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var victim = await GetByIdAsync(id);
            if (victim != null)
            {
                Context.Set<TDbModel>().Remove(victim);
                await Context.SaveChangesAsync();
            }       
        }

    }
}

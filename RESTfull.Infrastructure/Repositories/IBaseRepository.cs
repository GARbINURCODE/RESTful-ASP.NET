using RESTfull.Domain.Models;

namespace RESTfull.Infrastructure.Repositories
{
    public interface IBaseRepository<TDbModel> where TDbModel : BaseModel
    {
        public Task<List<TDbModel>> GetAllAsync();
        public Task <TDbModel?> GetByIdAsync(Guid id);
        public Task<TDbModel> CreateAsync(TDbModel model);
        public Task UpdateAsync(TDbModel model);
        public Task DeleteByIdAsync(Guid id);
    }
}

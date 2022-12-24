using Microsoft.AspNetCore.Mvc;
using RESTfull.Infrastructure.DTOs;

namespace RESTfull.API.Controllers
{
    public interface IMainController 
    {
        public Task<IActionResult> GetAllAsync();
        public Task<IActionResult> GetByIdAsync(Guid id);
        public Task<IActionResult> CreateAsync(BaseDTO basedto) { throw new NotImplementedException(); }
        public Task<IActionResult> CloseAsync(Guid id);
        public Task<IActionResult> DeleteAsync(Guid id);
    }
}

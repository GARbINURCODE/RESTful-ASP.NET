using Microsoft.AspNetCore.Mvc;
using RESTfull.Domain.Models.Products;
using RESTfull.Infrastructure.Repositories;
using RESTfull.Infrastructure.Data;
using RESTfull.Infrastructure.DTOs;

namespace RESTfull.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IPadController : ControllerBase, IMainController
    {
        private readonly ProductRepository<IPad> Repository;
        private readonly PersonRepository PersonRepository;

        public IPadController(Context context)
        {
            Repository = new ProductRepository<IPad>(context);
            PersonRepository = new PersonRepository(context);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            List<IPad> ipads = await Repository.GetAllAsync();
            var results = new List<ProductDTO>();
            foreach(IPad ipad in ipads) { results.Add(new ProductDTO(ipad)); }
            return Ok(results);
        }

        [HttpGet("(id)")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var ipad = await Repository.GetByIdAsync(id);
            if (ipad == null) { return BadRequest(); }
            else
            {
                ProductDTO result = new(ipad);
                return Ok(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(ProductDTO newipad)
        {
            var person = await PersonRepository.GetByIdAsync(newipad.PersonId);
            IPad ipad = new()
            {
                Id = Guid.NewGuid(),
                Person = person,
                Model = newipad.Model,
                Color = newipad.Color,
                Processor = newipad.Processor,
                RAM = newipad.RAM,
                Storage = newipad.Storage,
                Status = true
            };
            await Repository.CreateAsync(ipad);
            return Ok(newipad);
        }

        [HttpPut]
        public async Task<IActionResult> CloseAsync(Guid id)
        {
            var ipad = await Repository.GetByIdAsync(id);
            if (ipad == null) { return BadRequest(); }
            else
            {
                ipad.Status = false;
                await Repository.UpdateAsync(ipad);
                return Ok();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await Repository.DeleteAsync(id);
            return Ok();
        }
    }
}

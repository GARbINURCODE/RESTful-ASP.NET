using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTfull.Domain.Models;
using RESTfull.Infrastructure;
using RESTfull.Infrastructure.DTOs;
using RESTfull.Infrastructure.Data;
using System.Text.Json.Serialization;
using RESTfull.Infrastructure.Repositories;
using Microsoft.CodeAnalysis.CSharp;
using RESTfull.Domain.Models.Products;

namespace RESTfull.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PersonRepository Repository;
        private readonly ProductRepository<Product> ProductRepository;
        public PersonController(Context context) 
        {  
            Repository = new PersonRepository(context);
            ProductRepository = new ProductRepository<Product>(context);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            List<Person> persons = await Repository.GetAllAsync();
            var results = new List<PersonDTO>();
            foreach (Person person in persons) { results.Add(new PersonDTO(person)); }
            return Ok(results);
        }

        [HttpGet("(id)")]
        public async Task<IActionResult> GetOrdersByIdAsync(Guid id)
        {
            var person = await Repository.GetByIdAsync(id);
            if (person == null) { return BadRequest(); }
            else
            {
                PersonDTO persondto = new PersonDTO(person);
                List<Product> products = await ProductRepository.GetByPersonAsync(person);
                List<ProductDTO> result = new List<ProductDTO>();
                foreach (Product product in products) { result.Add(new ProductDTO(product)); }
                return Ok(result);
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(PersonDTO newperson)
        {
            Person person = new()
            {
                Id = Guid.NewGuid(),
                Name = newperson.Name,
                Surname = newperson.Surname,
                Email = newperson.Email,
                Phone = newperson.Phone,
                Orders = new List<Product>()
            };

            await Repository.CreateAsync(person);

            return Ok(newperson);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(Guid id) // Метод для удаления персоны по Id
        {
            await Repository.DeleteAsync(id);
            return Ok();
        }

    }
}

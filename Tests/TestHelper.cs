using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using RESTfull.Domain.Models;
using RESTfull.Domain.Models.Products;
using RESTfull.Infrastructure.Data;
using RESTfull.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class TestHelper
    {
        private readonly Context _context;

        public TestHelper()
        {
            var contextOptions = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new Context(contextOptions);

            _context.Database.EnsureCreated();
            _context.Database.EnsureDeleted();

            var personA = new Person()
            {
                Id = Guid.Parse("{6EF28B9F-A4C9-4E36-A74E-7E00B0FC029E}"),
                Name = "Vladimir",
                Surname = "Putin",
                Email = "IMZBest@mail.ru",
                Phone = "777777777777"
            };

            personA.AddMacBook(new MacBook()
            {
                Person = personA,
                Model = "Air 16",
                Color = "Silver",
                Processor = "M1",
                RAM = 16,
                Storage = 512
            });

            _context.Persons.Add(personA);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        public PersonRepository PersonRepository
        {
            get
            {
                return new PersonRepository(_context);
            }
        }
    }
}

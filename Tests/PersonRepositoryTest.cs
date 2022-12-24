using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using RESTfull.Domain.Models;
using RESTfull.Domain.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class PersonRepositoryTest
    {
        [Fact]
        public void DbCreationTest()
        {
            //Arrange
            var testHelper = new TestHelper();
            var personRepository = testHelper.PersonRepository;

            //Assert
            Assert.Equal(1, 1);
        }

        [Fact]
        public void CreateTest()
        {
            //Arrange
            var testHelper = new TestHelper();
            var personRepository = testHelper.PersonRepository;
            var person = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Ivan",
                Surname = "Govnov",
                Email = "IvanGovnov@gmail.com",
                Phone = "89123043465"
            };

            //Act
            personRepository.CreateAsync(person).Wait();
            personRepository.ChangeTrackerClear();

            //Assert
            Assert.True(personRepository.GetAllAsync().Result.Count == 2);
            Assert.NotNull(personRepository.GetByNameAsync("Ivan").Result);
            Assert.NotNull(personRepository.GetByNameAsync("Vladimir").Result);
            Assert.NotNull(personRepository.GetByNameAsync("Vladimir").Result.Orders);
        }

        [Fact]
        public void UpdateTest()
        {
            //Arrange
            var testHelper = new TestHelper();
            var personRepository = testHelper.PersonRepository;
            var person = personRepository.GetByNameAsync("Vladimir").Result;

            //Act
            person.Name = "Vladimir The Great";
            var Iphone = new IPhone()
            {
                Id = Guid.NewGuid(),
                Person = person,
                Model = "14 ProMax",
                Color = "Purple",
                Processor = "A2000 Bionic",
                RAM = 32,
                Storage = 1024
            };
            person.AddIPhone(Iphone);
            personRepository.UpdateAsync(person).Wait();

            //Assert
            Assert.NotNull(personRepository.GetByNameAsync("Vladimir The Great").Result.Name);
            Assert.Equal(2, personRepository.GetByNameAsync("Vladimir The Great").Result.Orders.Count);
        }

        [Fact]
        public void DeleteTest()
        {
            //Arrange
            var testHelper = new TestHelper();
            var personRepository = testHelper.PersonRepository;
            var person = personRepository.GetByNameAsync("Vladimir").Result;
            personRepository.ChangeTrackerClear();

            //Act
            person.Orders.RemoveAt(0);
            personRepository.UpdateAsync(person).Wait();

            //Assert
            Assert.Equal(1, personRepository.GetByNameAsync("Vladimir").Result.Orders.Count);
        }
    }
}

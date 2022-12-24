using RESTfull.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTfull.Infrastructure.DTOs
{
    public class PersonDTO : BaseDTO
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public PersonDTO(Person person)
        {
            Id = person.Id;
            Name = person.Name;
            Surname = person.Surname;
            Email = person.Email;
            Phone = person.Phone;
        }

        public PersonDTO() : base() { }
    }
}

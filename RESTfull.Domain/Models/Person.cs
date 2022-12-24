using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RESTfull.Domain.Models.Products;

namespace RESTfull.Domain.Models
{
    public class Person : BaseModel
    {
        public string? Name { get; set; } // Поле Имени персоны
        public string? Surname { get; set; } // Поле Фамилии персоны
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public List<Product> Orders { get; set; } = new List<Product>(); // Поле коллекции заказов персоны


        // Методы для добавления заказов каждого вида техники
        public void AddIPad(IPad ipad) { Orders.Add(ipad); }

        public void AddIPhone(IPhone iphone) { Orders.Add(iphone); }

        public void AddMacBook(MacBook macbook) { Orders.Add(macbook); }

        // Поле для удаления заказа по индексу
        public void RemoveAt(int index) { Orders.RemoveAt(index); }


    }
}

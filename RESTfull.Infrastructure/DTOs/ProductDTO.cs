using RESTfull.Domain.Models;
using RESTfull.Domain.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTfull.Infrastructure.DTOs
{
    public class ProductDTO : BaseDTO
    {
        public Guid PersonId { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public string? Processor { get; set; }
        public byte RAM { get; set; }
        public ushort Storage { get; set; }

        public ProductDTO(Product product)
        {
            Id = product.Id;
            PersonId = product.Person!.Id;
            Model = product.Model;
            Color = product.Color;
            Processor = product.Processor;
            RAM = product.RAM;
            Storage = product.Storage;
        }

        public ProductDTO() : base() { }
    }
}

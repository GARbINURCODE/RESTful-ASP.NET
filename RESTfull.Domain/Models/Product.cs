using System.Runtime.CompilerServices;

namespace RESTfull.Domain.Models
{
    public abstract class Product : BaseModel
    {
        public Person? Person { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public string? Processor { get; set; }
        public byte RAM { get; set; }
        public ushort Storage { get; set; }
        public bool Status { get; set; }
    }
}
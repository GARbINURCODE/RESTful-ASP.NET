using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using RESTfull.Domain.Models;
using RESTfull.Domain.Models.Products;

namespace RESTfull.Infrastructure.Data
{
    public class Context : DbContext
    {

         public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<IPhone> IPhones { get; set; }
        public DbSet<IPad> IPads { get; set; }
        public DbSet<MacBook> MacBooks { get; set; }

    }
}
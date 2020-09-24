using DremelPrinterBridge.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DremelPrinterBridge.Database
{
    public class InMemoryContextDb : DbContext
    {
        public InMemoryContextDb(DbContextOptions<InMemoryContextDb> options) : base(options)
        {
        }

        public DbSet<Printer> Printers { get; set; }    }
}

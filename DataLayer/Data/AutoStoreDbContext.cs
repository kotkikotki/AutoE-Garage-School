using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LogicLayer.Models;

namespace DataLayer
{
    public class AutoStoreDbContext : DbContext
    {
        public AutoStoreDbContext(DbContextOptions<AutoStoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<LogicLayer.Models.Vehicle> Vehicles { get; set; } = default!;
    }
}
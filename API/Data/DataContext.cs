using System.Data.Common;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions options): base(options)// this is the constructor for db context
        {

        }
        public DbSet<AppUser> Users { get; set; } // we are adding the db set 
    }
}
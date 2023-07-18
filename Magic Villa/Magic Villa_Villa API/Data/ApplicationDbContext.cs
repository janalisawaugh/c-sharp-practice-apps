using Magic_Villa_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace Magic_Villa_VillaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }
        public DbSet<Villa> Villas { get; set; }
    }
}

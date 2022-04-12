using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ThaLeague.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ThaLeague.Models.Artist>? Artist { get; set; }
        public DbSet<ThaLeague.Models.Audio>? Audio { get; set; }
        public DbSet<ThaLeague.Models.Video>? Video { get; set; }
    }
}
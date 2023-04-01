using System.ComponentModel.DataAnnotations;
using kDg.FileBaseContext.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class FileContext : DbContext
    {
        public DbSet<Callback> Callbacks { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Default: JSON-Serializer
            optionsBuilder.UseFileBaseContextDatabase(location: "/Data");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Callback>()
                .ToTable("callbacks");
        }
    }

    public class Callback {
        [Key]
        public Guid id {get; set;}
        public required string url {get; set;}
        public DateTime timeof {get; set;} 
        public bool IsComplete {get; set;}  = false;
    }

    public class CallbackCreate_Dto {
        public required string url {get; set;}  = "";
        public required string timeof {get; set;} = "";
        public static Callback ToDB(CallbackCreate_Dto input) {
            return new Callback {
                url = input.url,
                timeof = DateTime.Parse(input.timeof)
            };
        } 
    }
}
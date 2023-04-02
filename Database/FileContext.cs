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
        public string json {get; set;} = "";
        public string form {get; set;} = "";
        public string method {get; set;} = "";
        public string response {get; set;} = "";
    }

    public class CallbackCreate_Dto {
        public required string url {get; set;}  = "";
        public required string timeof {get; set;} = "";
        public required string method {get; set;} = "POST";
        public string json {get;set; } = "";
        public string form {get; set;} = "";
        public static Callback ToDB(CallbackCreate_Dto input) {
            return new Callback {
                url = input.url,
                timeof = DateTime.Parse(input.timeof),
                json  = input.json,
                form = input.form,
                method = input.method
            };
        } 
    }
}
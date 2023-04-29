using Infrastructure;
using kDg.FileBaseContext.Extensions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class Context : DbContext
{
    private ILogger<Context> _logger;

    public Context(DbContextOptions<Context> options, ILogger<Context> logger) : base(options) {
        _logger = logger;        
    }
    public DbSet<Callback> Callbacks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        SetupFolders();        
        switch(TheConfiguration.DatabaseType) {
            case DatabaseType.FileSystem:
                Setup_FileSystem(optionsBuilder);
                break;
            case DatabaseType.SqlLiteInMemory:
                Setup_SqlLiteInMemory(optionsBuilder);
                break;
            case DatabaseType.SqlLite:
                Setup_SqlLite(optionsBuilder);
                break;
        }
    }

    private const string dataPath = "/Data";
    private const string sqlitePath = dataPath + "/sqlite";    
    private const string jsonPath = dataPath + "/json";
    private void SetupFolders()
    {
        SetupAFolder(dataPath);
        SetupAFolder(jsonPath);
        SetupAFolder(sqlitePath);
    }

    private void SetupAFolder(string path) {
        if (!Directory.Exists(path))
        {
        Directory.CreateDirectory(path);
        }
    }


    private void Setup_FileSystem(DbContextOptionsBuilder opt) {
        _logger.LogInformation("Setup_FileSystem_start");
        opt.UseFileBaseContextDatabase(location: "/Data");
        _logger.LogInformation("Setup_FileSystem_end");
    }

    private void Setup_SqlLiteInMemory(DbContextOptionsBuilder opt) {
        _logger.LogInformation("Setup_SqlLiteInMemory_start");
        var _connection = new SqliteConnection("Data Source=InMemorySample;Mode=Memory;Cache=Shared");
        _connection.Open();
        opt.UseSqlite(connection: _connection);
        _logger.LogInformation("Setup_SqlLiteInMemory_end");
    }

    private void Setup_SqlLite(DbContextOptionsBuilder opt) {
        var _connection = new SqliteConnection($"Data Source={sqlitePath}/callbacks.db");
        _connection.Open();
        opt.UseSqlite(connection: _connection);
    }
}
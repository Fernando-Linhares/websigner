using System.Net;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using ModelFile=Api.Models.File;

namespace Api.Database;

public class DataContext: DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<ModelFile> Files { get; set; }
    public DbSet<Certificate> Certificates { get; set; }
    public DbSet<Token> Tokens { get; set; }
    public ConfigurationBuilder ConfigurationBuilder { get; } = new();

    public DataContext(DbContextOptions<DataContext> options): base(options) {}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigApp();
        optionsBuilder.UseSqlServer(config.Get("db.connection"));
        base.OnConfiguring(optionsBuilder);
    }
}
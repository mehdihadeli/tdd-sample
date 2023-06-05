using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace TDDSample.Shared.Data;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options) { }

    public DbSet<TodoItem.Models.TodoItem> TodoItems => Set<TodoItem.Models.TodoItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        SeedData(builder);
    }

    private static void SeedData(ModelBuilder builder)
    {
        for (var i = 1; i <= 20; i++)
        {
            builder
                .Entity<TodoItem.Models.TodoItem>()
                .HasData(
                    new TodoItem.Models.TodoItem
                    {
                        Id = i,
                        Title = $"Todo Item {i}",
                        IsCompleted = false,
                        CreatedOn = DateTime.UtcNow,
                        UserId = 1
                    }
                );
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TDDSample.Shared.Data;

public class TodoDbContextFactory : IDesignTimeDbContextFactory<TodoDbContext>
{
    public TodoDbContext CreateDbContext(params string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TodoDbContext>();

        if (optionsBuilder.IsConfigured)
            return new TodoDbContext(optionsBuilder.Options);

        optionsBuilder.UseNpgsql(
            "Server=127.0.0.1;Initial Catalog=TodoDatabase;Persist Security Info=False;User ID=postgres;Password=postgres;"
        );

        return new TodoDbContext(optionsBuilder.Options);
    }
}

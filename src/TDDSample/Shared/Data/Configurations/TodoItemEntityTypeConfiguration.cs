using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TDDSample.Shared.Data.Configurations;

public class TodoItemEntityTypeConfiguration : IEntityTypeConfiguration<TodoItem.Models.TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem.Models.TodoItem> builder)
    {
        builder.ToTable("TodoItems");
        builder.HasKey(x => x.Id);
    }
}

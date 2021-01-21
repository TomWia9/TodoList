using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Server.Models;

namespace TodoList.Server.Data.EntityConfiguration
{
    public class ListOfTodosConfiguration : IEntityTypeConfiguration<ListOfTodos>
    {
        public void Configure(EntityTypeBuilder<ListOfTodos> builder)
        {
            builder.HasKey(l => l.Id);

            builder.HasMany(l => l.Todos)
                .WithOne(t => t.ListOfTodos)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(l => l.Title)
                //.IsRequired() this rule is set in model as data annotation
                .HasMaxLength(100);
        }
    }
}

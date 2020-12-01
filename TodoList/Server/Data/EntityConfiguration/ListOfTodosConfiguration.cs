using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}

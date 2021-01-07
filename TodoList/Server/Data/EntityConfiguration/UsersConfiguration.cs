using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Server.Models;

namespace TodoList.Server.Data.EntityConfiguration
{
    public class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired() 
                .HasMaxLength(20);

            builder.Property(u => u.Password)
                .HasMaxLength(64);

            builder.HasMany(u => u.ListOfTodos)
                .WithOne(l => l.User)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

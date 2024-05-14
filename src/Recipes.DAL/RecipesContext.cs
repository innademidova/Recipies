﻿using Microsoft.EntityFrameworkCore;
using Recipes.DAL.Models;

namespace Recipes.DAL;

public sealed class RecipesContext : DbContext
{
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<User> Users => Set<User>();

    public RecipesContext(DbContextOptions<RecipesContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.Property(user => user.Email).HasMaxLength(1000);
            builder.HasIndex(user => user.Email).IsUnique();
            builder.HasOne(user => user.UserAccount)
                .WithOne()
                .HasForeignKey<UserAccount>(r => r.UserId);

            builder.Property(user => user.FirstName).HasMaxLength(300);
            builder.Property(user => user.LastName).HasMaxLength(300);
        });

        modelBuilder.Entity<UserAccount>(builder =>
        {
            builder.Property(userAccount => userAccount.Role)
                .HasConversion<string>()
                .HasMaxLength(100);

            builder.Property(userAccount => userAccount.Password).HasMaxLength(100);
        });


    }
}
using Entities.Configuration;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Code for many to many relationship on a self-referencing person table
            modelBuilder.Entity<PersonRelation>()
                    .HasKey(e => new { e.RelatedFromId, e.RelatedToId });

            modelBuilder.Entity<PersonRelation>()
                .HasOne(e => e.PersonRelatedFrom)
                .WithMany(e => e.RelatedTo)
                .HasForeignKey(e => e.RelatedFromId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PersonRelation>()
                .HasOne(e => e.PersonRelatedTo)
                .WithMany(e => e.RelatedFrom)
                .HasForeignKey(e => e.RelatedToId)
                .OnDelete(DeleteBehavior.Restrict);

            //Setting seed data for each entity
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new CityConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneNumberConfiguration());
            modelBuilder.ApplyConfiguration(new PersonRelationConfiguration());
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }

        public DbSet<PersonRelation> PersonRelations { get; set; }
    }
}

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
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }

        public DbSet<PersonRelation> PersonRelations { get; set; }
    }
}

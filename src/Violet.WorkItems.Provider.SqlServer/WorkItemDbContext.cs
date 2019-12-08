using System;
using Microsoft.EntityFrameworkCore;

namespace Violet.WorkItems.Provider.SqlServer
{
    public class WorkItemDbContext : DbContext
    {
        private readonly string _connectionString;

        public WorkItemDbContext()
        {
            _connectionString = @"Server=localhost\SQLEXPRESS;Database=workitems;Trusted_Connection=True;";
        }

        public WorkItemDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkItem>()
                .HasKey(nameof(WorkItem.ProjectCode), nameof(WorkItem.Id));

            modelBuilder.Entity<WorkItem>(workItemEntityBuilder =>
            {
                workItemEntityBuilder.Property(e => e.ProjectCode);
                workItemEntityBuilder.Property(e => e.Id);
                workItemEntityBuilder.Property(e => e.WorkItemType);
            });

            modelBuilder.Entity<WorkItem>()
                .OwnsMany(e => e.Properties, propertyEntityBuilder =>
                {
                    propertyEntityBuilder.Property(p => p.Name);
                    propertyEntityBuilder.Property(p => p.DataType);
                    propertyEntityBuilder.Property(p => p.Value);

                    // shadow properties
                    propertyEntityBuilder.Property<string>(nameof(WorkItem.ProjectCode));
                    propertyEntityBuilder.Property<string>(nameof(WorkItem.Id));

                    propertyEntityBuilder.WithOwner().HasForeignKey(nameof(WorkItem.ProjectCode), nameof(WorkItem.Id));
                    propertyEntityBuilder.HasKey(nameof(WorkItem.ProjectCode), nameof(WorkItem.Id), nameof(Property.Name));
                });

            modelBuilder.Entity<WorkItem>()
                .OwnsMany(e => e.Log, logEntityBuilder =>
                {
                    logEntityBuilder.Property(p => p.Date);
                    logEntityBuilder.Property(p => p.User);
                    logEntityBuilder.Property(p => p.Comment);

                    // shadow properties
                    logEntityBuilder.Property<string>(nameof(WorkItem.ProjectCode));
                    logEntityBuilder.Property<string>(nameof(WorkItem.Id));

                    logEntityBuilder.WithOwner().HasForeignKey(nameof(WorkItem.ProjectCode), nameof(WorkItem.Id));
                    logEntityBuilder.HasKey(nameof(WorkItem.ProjectCode), nameof(WorkItem.Id), nameof(LogEntry.Date));

                    logEntityBuilder.OwnsMany(e => e.Changes, propertyChangeEntityBuilder =>
                    {
                        propertyChangeEntityBuilder.Property(p => p.Name);
                        propertyChangeEntityBuilder.Property(p => p.OldValue);
                        propertyChangeEntityBuilder.Property(p => p.NewValue);

                        // shadow properties
                        propertyChangeEntityBuilder.Property<string>(nameof(WorkItem.ProjectCode));
                        propertyChangeEntityBuilder.Property<string>(nameof(WorkItem.Id));
                        propertyChangeEntityBuilder.Property<DateTimeOffset>(nameof(LogEntry.Date));

                        propertyChangeEntityBuilder.WithOwner().HasForeignKey(nameof(WorkItem.ProjectCode), nameof(WorkItem.Id), nameof(LogEntry.Date));
                        propertyChangeEntityBuilder.HasKey(nameof(WorkItem.ProjectCode), nameof(WorkItem.Id), nameof(LogEntry.Date), nameof(PropertyChange.Name));
                    });
                });
        }

        public DbSet<WorkItem> WorkItems { get; set; }
    }
}

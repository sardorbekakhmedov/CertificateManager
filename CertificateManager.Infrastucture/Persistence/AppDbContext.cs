using CertificateManager.Application.Abstractions.Interfaces;
using CertificateManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CertificateManager.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<CertificateManager.Domain.Entities.Certificate> Certificates { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateTimeStampForBaseEntityClass();
        return base.SaveChangesAsync(cancellationToken);
    }
    public async Task BeginTransactionAsync()
    {
        await Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await Database.RollbackTransactionAsync();
    }

    private void UpdateTimeStampForBaseEntityClass()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is not AuditableEntity entity)
                continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    entity.CreatedDate = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entity.LastModifiedDate = DateTime.UtcNow;
                    break;
            }
        }
    }

  

}
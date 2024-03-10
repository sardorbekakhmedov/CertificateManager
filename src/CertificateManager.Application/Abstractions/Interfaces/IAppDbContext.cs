using CertificateManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CertificateManager.Application.Abstractions.Interfaces;

public interface IAppDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<CertificateManager.Domain.Entities.Certificate> Certificates { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    public Task BeginTransactionAsync();

    public Task CommitTransactionAsync();

    public Task RollbackTransactionAsync();
}
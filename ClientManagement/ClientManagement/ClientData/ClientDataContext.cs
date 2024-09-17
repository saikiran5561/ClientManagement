using System;
using System.Collections.Generic;
using ClientManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.ClientData;

public partial class ClientDataContext : ClientManagementContext
{
    public ClientDataContext(DbContextOptions<ClientDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.Property(e => e.ClientName).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

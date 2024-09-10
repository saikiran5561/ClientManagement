using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.ClientData;

public partial class ClientDataContext : DbContext
{
    public ClientDataContext()
    {
    }

    public ClientDataContext(DbContextOptions<ClientDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=localhost;Database=ClientData;Trusted_Connection=True;TrustServerCertificate=True");

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

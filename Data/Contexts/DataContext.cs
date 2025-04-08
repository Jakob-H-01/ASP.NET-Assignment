using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<MemberEntity>(options)
{
    public virtual DbSet<StatusEntity> Status { get; set; }
    public virtual DbSet<ClientEntity> Clients { get; set; }
    public virtual DbSet<ProjectEntity> Projects { get; set; }
}

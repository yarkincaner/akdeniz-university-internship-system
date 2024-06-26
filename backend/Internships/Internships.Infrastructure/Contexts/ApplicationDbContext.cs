using Internships.Core.Interfaces;
using Internships.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Infrastructure.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Internship> Internships { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<InternshipStatus> InternshipStatuses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status>Statuses { get; set; }
        public DbSet<UserRoles> UserRoleses { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTime.NowUtc;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.NowUtc;
                        entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Company>().
                HasMany(e => e.Employees).
                WithOne(e => e.Company).
                OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Company>().
                HasMany(e => e.Internships).
                WithOne(e => e.Company).
                OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Employee>().
                HasOne(e => e.Company).
                WithMany(e => e.Employees).
                HasForeignKey(e => e.CompanyId).
                OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Internship>().
                HasOne(e=>e.Company).
                WithMany(e=>e.Internships).
                HasForeignKey(e=>e.CompanyId).
                OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Department>().
                HasMany(e => e.Users).
                WithOne(e => e.Departmant).
                OnDelete(DeleteBehavior.NoAction);

            builder.Entity<User>().
                HasOne(e => e.Departmant).
                WithMany(e => e.Users).
                HasForeignKey(e => e.DepartmentID).
                OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Document>().
                HasOne(e => e.Internship).
                WithMany(e => e.Documents).
                OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Document>().
                HasOne(e => e.DocumentType).
                WithMany(e => e.Documents).
                OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Internship>().
                HasMany(e => e.Documents).
                WithOne(e => e.Internship).
                HasForeignKey(e => e.InternshipId).
                OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DocumentType>().
                HasMany(e => e.Documents).
                WithOne(e => e.DocumentType).
                HasForeignKey(e => e.DocumentTypeId).
                OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Employee>().
                HasMany(e => e.Internships).
                WithOne(e => e.Employee).
                HasForeignKey(e => e.EmployeeId).
                OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Internship>().
                HasMany(e => e.InternshipStatuses).
                WithOne(e => e.Internship).
                OnDelete(DeleteBehavior.NoAction);

            builder.Entity<InternshipStatus>().
                HasOne(e => e.Internship).
                WithMany(e => e.InternshipStatuses).
                HasForeignKey(e => e.InternshipId).
                OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserRoles>().
                HasOne(e => e.Role).
                WithOne(e => e.UserRoles).
                HasForeignKey<UserRoles>(e => e.RoleId).
                OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserRoles>().
                HasOne(e => e.User).
                WithOne(e => e.UserRoles).
                HasForeignKey<UserRoles>(e => e.UserId).
                OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Status>().
                HasMany(e => e.InternshipStatuses).
                WithOne(e => e.Status).
                HasForeignKey(e => e.StatusId).
                OnDelete(DeleteBehavior.NoAction);

        }
    }
}

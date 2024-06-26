using Internships.Core.Interfaces;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Settings;
using Internships.Infrastructure.Contexts;
using Internships.Infrastructure.Repositories;
using Internships.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Internships.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                      options.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection"),
                   b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
            
        
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.Configure<AWSSimpleEmailServiceSettings>(configuration.GetSection("AWSSimpleEmailServiceSettings"));
            services.AddTransient<IDateTimeService, DateTimeService>();

            services.AddScoped<IBlobStorageService, BlobStorageService>();
            services.Configure<BlobStorageSettings>(configuration.GetSection("BlobStorageSettings"));
            services.AddTransient<IEmailService, AWSMailService>();

            services.Configure<TokenSettings>(configuration.GetSection("TokenSettings"));
            services.AddTransient<IExternalAccountService, ExternalAccountService>();
            

            #region Repositories

            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<ICompanyRepositoryAsync, CompanyRepositoryAsync>();
            services.AddTransient<IInternshipRepositoryAsync, InternshipRepositoryAsync>();
            services.AddTransient<IEmployeeRepositoryAsync, EmployeeRepositoryAsync>();
            services.AddTransient<IDepartmentRepositoryAsync, DepartmentRepositoryAsync>();
            services.AddTransient<IUserRepositoryAsync, UserRepositoryAsync>();
            services.AddTransient<IInternshipStatusRepositoryAsync, InternshipStatusRepositoryAsync>();
            services.AddTransient<IStatusRepositoryAsync, StatusRepositoryAsync>();
            services.AddTransient<IDocumentRepositoryAsync, DocumentRepositoryAsync>();

            #endregion

        }
    }
}

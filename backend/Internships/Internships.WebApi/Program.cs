using AutoMapper;
using Internships.Core;
using Internships.Core.Interfaces;
using Internships.Core.Mappings;
using Internships.Infrastructure;
using Internships.WebApi.Extensions;
using Internships.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ProductionPolicy",
        builder =>
        {
            builder.WithOrigins("https://aybs.akdeniz.edu.tr", "https://internship-csart-arges-projects.vercel.app")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


//Add configurations
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true);

// Add services to the container.
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
// Add AutoMapper configuration
builder.Services.AddAutoMapper(typeof(GeneralProfile));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
        {
            builder.Configuration.Bind("AzureAd", options);
            options.TokenValidationParameters.NameClaimType = "name";
        },
        options => { builder.Configuration.Bind("AzureAd", options); }
    );

builder.Services.AddSwaggerExtension();
builder.Services.AddControllers();
builder.Services.AddApiVersioningExtension();
builder.Services.AddHealthChecks();
builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();


builder.Configuration.GetSection("BlobStorageSettings");


//Build the application
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
}
else
{
    app.UseCors("ProductionPolicy");
}

app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerExtension();
app.UseErrorHandlingMiddleware();
app.UseHealthChecks("/health");

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


//Initialize Logger

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(app.Configuration)
    .CreateLogger();


//Start the application
app.Run();
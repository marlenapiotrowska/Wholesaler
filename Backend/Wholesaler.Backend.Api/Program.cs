using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Wholesaler.Backend.Api;
using Wholesaler.Backend.Api.Factories;
using Wholesaler.Backend.Api.Factories.Interfaces;
using Wholesaler.Backend.DataAccess;
using Wholesaler.Backend.DataAccess.Factories;
using Wholesaler.Backend.DataAccess.Repositories;
using Wholesaler.Backend.Domain.Factories;
using Wholesaler.Backend.Domain.Factories.Interfaces;
using Wholesaler.Backend.Domain.Interfaces;
using Wholesaler.Backend.Domain.Providers.Interfaces;
using Wholesaler.Backend.Domain.Repositories;
using Wholesaler.Backend.Domain.Services;
using ClientFactoryApi = Wholesaler.Backend.Api.Factories.ClientFactory;
using ClientFactoryDomain = Wholesaler.Backend.Domain.Factories.ClientFactory;
using IClientFactoryApi = Wholesaler.Backend.Api.Factories.Interfaces.IClientFactory;
using IClientFactoryDomain = Wholesaler.Backend.Domain.Factories.Interfaces.IClientFactory;
using IWorkTaskFactoryDataAccess = Wholesaler.Backend.DataAccess.Factories.IWorkTaskFactory;
using IWorkTaskFactoryDomain = Wholesaler.Backend.Domain.Factories.Interfaces.IWorkTaskFactory;
using WorkTaskFactoryDataAccess = Wholesaler.Backend.DataAccess.Factories.WorkTaskFactory;
using WorkTaskFactoryDomain = Wholesaler.Backend.Domain.Factories.WorkTaskFactory;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DBConnection");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WholesalerContext>(
    opt => opt
        .UseSqlServer(connection)
        .LogTo(
            l =>
            {
                if (l.Contains("CommandExecuting"))
                    Log.Logger.Information(l);
            }));

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowVueApp",
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

builder.Host.UseSerilog((_, configuration) => configuration
    .WriteTo.File(new CompactJsonFormatter(), "logs/log.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext());

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddTransient<IWorkdayRepository, WorkdayRepository>();
builder.Services.AddTransient<IWorkTaskRepository, WorkTaskRepository>();
builder.Services.AddTransient<IWorkTaskService, WorkTaskService>();
builder.Services.AddTransient<IWorkTaskFactoryDomain, WorkTaskFactoryDomain>();
builder.Services.AddTransient<IWorkTaskFactoryDataAccess, WorkTaskFactoryDataAccess>();
builder.Services.AddTransient<IWorkdayFactory, WorkdayFactory>();
builder.Services.AddTransient<IPersonDbFactory, PersonDbFactory>();
builder.Services.AddTransient<IWorkTaskDtoFactory, WorkTaskDtoFactory>();
builder.Services.AddTransient<IWorkdayDtoFactory, WorkdayDtoFactory>();
builder.Services.AddTransient<IUserDtoFactory, UserDtoFactory>();
builder.Services.AddTransient<IPersonDbFactory, PersonDbFactory>();
builder.Services.AddTransient<IPersonFactory, PersonFactory>();
builder.Services.AddTransient<IClientFactoryDomain, ClientFactoryDomain>();
builder.Services.AddTransient<IClientFactoryApi, ClientFactoryApi>();
builder.Services.AddTransient<IStorageDbFactory, StorageDbFactory>();
builder.Services.AddTransient<IStorageRepository, StorageRepository>();
builder.Services.AddTransient<IStorageFactory, StorageFactory>();
builder.Services.AddTransient<IStorageDtoFactory, StorageDtoFactory>();
builder.Services.AddTransient<IClientDbFactory, ClientDbFactory>();
builder.Services.AddTransient<IStorageService, StorageService>();
builder.Services.AddTransient<IRequirementFactory, RequirementFactory>();
builder.Services.AddTransient<IRequirementDtoFactory, RequirementDtoFactory>();
builder.Services.AddTransient<IRequirementDbFactory, RequirementDbFactory>();
builder.Services.AddTransient<IClientService, ClientService>();
builder.Services.AddTransient<IRequirementService, RequirementService>();
builder.Services.AddTransient<IClientRepository, ClientRepository>();
builder.Services.AddTransient<IRequirementRepository, RequirementRepository>();
builder.Services.AddTransient<IActivityRepository, ActivityRepository>();
builder.Services.AddTransient<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddTransient<IRaportService, RaportService>();
builder.Services.AddTransient<IDeliveryFactory, DeliveryFactory>();
builder.Services.AddTransient<IRoleInfoFactory, RoleInfoFactory>();
builder.Services.AddTransient<ITimeProvider, Wholesaler.Barckend.Domain.Providers.TimeProvider>();
builder.Services.AddScoped<ITransaction, Transaction>();
builder.Services.AddTransient<ErrorHandlingMiddleware>();
builder.Services.AddTransient<RequestLoggingMiddleware>();
//builder.Services.AddHostedService<TimedHostedService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/openapi.json", "Wholesaler API v1");
    c.RoutePrefix = "swagger";
});

app.UseStaticFiles();
app.UseDatabase();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseCors("AllowVueApp");
app.MapControllers();
app.Run();

public partial class Program
{
}

using Amazon;
using Amazon.SimpleNotificationService;
using Mbiza.Bank;
using Mbiza.Bank.Aws;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<MbizaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MbizaDB"));
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Liso Mbiza - Bank Software Api v1", Version = "v1" });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

builder.Services.Configure<AmazonConfigSettings>(builder.Configuration.GetSection("ConfigSettings"));

builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IAccountsRepository, AccountsRepository>();
builder.Services.AddTransient<IAmazonPublishService, AmazonPublishService>();
builder.Services.AddSingleton<IAmazonSimpleNotificationService>(sp =>
{
    return new AmazonSimpleNotificationServiceClient(
        "<access_key>",
        "<secret_key>",
        RegionEndpoint.EUNorth1
    );
});
builder.Services.AddSingleton<ILoggerFactory>(sp =>
{
    return LoggerFactory.Create(logging =>
    {
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Information);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Liso Mbiza - Bank Software Api v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks("/health");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<MbizaContext>();
        context.Database.Migrate();

        if (!context.Account.Any())
        {
            context.Account.Add(new Accounts
            {
                Amount = 0,
                Balance = 25000m,
                CreatedDateTime = DateTime.Now,
                CreatedBy = "Liso Mbiza"
            });
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error applying migrations: {ex.Message}");
        throw;
    }
}

app.Run();

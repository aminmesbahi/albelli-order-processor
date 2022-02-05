using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Db");

builder.Services.AddDbContext<OrderProcessorDbContext>(x => x.UseSqlite(connectionString));
builder.Services.AddScoped<IOrderProcessingService, OrderProcessingService>();
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    setup.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    setup.EnableAnnotations();
    setup.SwaggerDoc("v1",
        new OpenApiInfo { Description = "Order Processor API", Title = "Albelli Assessment", Version = "v1" });
});

var app = builder.Build();

await EnsureDbAsync(app.Services);

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(options=>options.DefaultModelsExpandDepth(-1));


app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run();


static async Task EnsureDbAsync(IServiceProvider sp)
{
    await using var db = sp.CreateScope().ServiceProvider.GetRequiredService<OrderProcessorDbContext>();
    await db.Database.MigrateAsync();
}
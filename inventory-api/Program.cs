using inventory_api.Data;
using inventory_api.Interfaces;
using inventory_api.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson( o =>
{
    o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    o.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
    o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    //o.SerializerSettings.DateFormatString  
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Connections String
string connectionString = builder.Configuration.GetConnectionString("InventoryContext");

//DbContext DI
builder.Services.AddDbContext<InventoryDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);

//Services DI
builder.Services.AddTransient<IProductService, ProductService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Core.IRepositories;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// inject the dbcontext
var DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ECommerceDBContext>(Options =>
    Options.UseSqlServer(
            DefaultConnection,
            b => b.MigrationsAssembly(typeof(ECommerceDBContext).Assembly.FullName))
);

// 
#region inject the  repository
builder.Services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));
builder.Services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
#endregion



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

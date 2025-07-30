using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using WebApi.Data.Models;
using WebApi.Services;
using WebApi.Contracts.Interface;
using Swashbuckle.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddControllers();

// Add API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Optionally enable Swagger in production
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    // Seed data if empty
    if (!context.Employees.Any())
    {
        var employees = new[]
        {
            new Employee { Name = "John Doe", Email = "john.doe@example.com", CreatedAt = DateTime.UtcNow },
            new Employee { Name = "Jane Smith", Email = "jane.smith@example.com", CreatedAt = DateTime.UtcNow }
        };

        context.Employees.AddRange(employees);
        context.SaveChanges();
    }
    else
    {
        // Optionally, you can log that the database already exists
        Console.WriteLine("Database already exists, skipping creation.");
    }
}


app.Run();
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PropertyViewings.Application.Abstractions;
using PropertyViewings.Application.Features.Bookings;
using PropertyViewings.Application.Features.Search;
using PropertyViewings.Infrastructure.Data;
using PropertyViewings.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// EF InMemory (DbContext lives in Infrastructure)
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("PropertyViewings"));

// Repository + UseCases
builder.Services.AddScoped<IViewingRepository, EfViewingRepository>();
builder.Services.AddScoped<BookViewingUseCase>();
builder.Services.AddScoped<SearchAvailableSlotsUseCase>();

// FluentValidation (scans Application assembly)
builder.Services.AddValidatorsFromAssembly(typeof(BookViewingValidator).Assembly);

var app = builder.Build();

// Simple exception mapping (keeps demo clean)
app.Use(async (ctx, next) =>
{
    try
    {
        await next();
    }
    catch (FluentValidation.ValidationException ex)
    {
        ctx.Response.StatusCode = 400;
        await ctx.Response.WriteAsJsonAsync(new
        {
            error = "Validation failed",
            details = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
        });
    }
    catch (InvalidOperationException ex)
    {
        // e.g. slot already booked
        ctx.Response.StatusCode = 409;
        await ctx.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
});

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

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PropertyViewings.Api;
using PropertyViewings.Application.Abstractions;
using PropertyViewings.Application.Features.Bookings;
using PropertyViewings.Application.Features.Search;
using PropertyViewings.Infrastructure.Data;
using PropertyViewings.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Data (EF InMemory)
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("PropertyViewings"));

// Repositories + UseCases
builder.Services.AddScoped<IViewingRepository, EfViewingRepository>();
builder.Services.AddScoped<BookViewingUseCase>();
builder.Services.AddScoped<SearchAvailableSlotsUseCase>();

// Validation (scans Application assembly)
builder.Services.AddValidatorsFromAssembly(typeof(BookViewingValidator).Assembly);

var app = builder.Build();

app.UseApiExceptionMapping();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using MediatR;
using ModelUse.Data;
using ModelUse.Implementation.Cqrs;
using System.Reflection;
using FluentValidation.AspNetCore;
using ModelUse.Implementation.Validation;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers().AddFluentValidation(x =>
{
    x.RegisterValidatorsFromAssemblyContaining<BookRequestValidator>();
});
builder.Services.AddMediatR(x=> x.RegisterServicesFromAssemblies(typeof(CreateBookCommand).GetTypeInfo().Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRewriter(new RewriteOptions().AddRedirect("^$", "/swagger/index.html"));
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();


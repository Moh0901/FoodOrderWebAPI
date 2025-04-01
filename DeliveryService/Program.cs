using DeliveryService;
using DeliveryService.DatabaseContext;
using DeliveryService.Repository;
using DeliveryService.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DeliveryContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DeliveryConnection")));
builder.Services.AddScoped<IDeliverRepository, DeliveryRepository>();
builder.Services.AddScoped<IDeliveryService, DeliverService>();
builder.Services.AddHostedService<OrderConsumer>();

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

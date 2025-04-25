using Microsoft.EntityFrameworkCore;
using EmailService.Services;
using EmailService.DataObjects;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<EmailService.DataObjects.EmailDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add EmailService
builder.Services.AddScoped<EmailDbContext>();
builder.Services.AddScoped<IEmailService, EmailProcessingService>();

var app = builder.Build();

app.Run();

using Microsoft.EntityFrameworkCore;
using EmailService.Services;
using EmailService.DataObjects;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.Run();

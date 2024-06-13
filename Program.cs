using Blog.API.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlogDataContext>();


var app = builder.Build();

app.Run();

using AspBlog.Data;
using AspBlog.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddDbContext<DataContext>();  // Injeção de dependência para o DataContext
builder.Services.AddTransient<TokenService>();  // Injeção de dependência para o TokenService

var app = builder.Build();

app.MapControllers();

app.Run();

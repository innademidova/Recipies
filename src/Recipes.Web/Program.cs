using Microsoft.EntityFrameworkCore;
using Recipes.BLL.Services;
using Recipes.DAL;
using Recipes.Extensions;
using Recipes.Middlewares;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, lc) => lc
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("ApplicationName", "Recipes.Web")
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuthorization();
builder.Services.AddControllers();
builder.Services.AddDbContext<RecipesContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddFluentValidators();
builder.Services.AddJwtBearerAuthentication();
builder.Services.AddProblemDetails();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

app.UseMiddleware<RequestDurationMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.Run();
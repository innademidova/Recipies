using Microsoft.EntityFrameworkCore;
using Recipes.BLL.Services;
using Recipes.DAL;
using Recipes.Extensions;
using Recipes.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext());

builder.Services.AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerWithAuthorization()
    .AddDbContext<RecipesContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!))
    .AddFluentValidators()
    .AddJwtBearerAuthentication()
    .AddProblemDetails()
    .AddScoped<UserService>();

var app = builder.Build();

app.MapControllers();

app
    .InDevelopment(b =>
        b.UseSwagger().UseSwaggerUI())
    .UseMiddleware<RequestDurationMiddleware>()
    .Use(CustomMiddlewares.ExtendRequestDurationMiddleware)
    .UseSerilogRequestLogging()
    .UseHttpsRedirection()
    .UseAuthentication()
    .UseAuthorization()
    .UseExceptionHandler()
    .UseStatusCodePages();

app.InDevelopment(b =>
        b.UseDeveloperExceptionPage())
    .UseMiddleware<BusinessValidationMiddleware>();

app.Run();
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using TechTest.Data;
using TechTest.Data.Models;
using TechTest.Data.Repositories;

var builder = WebApplication.CreateSlimBuilder(args);

builder.AddServiceDefaults();

builder.Services.ConfigureHttpJsonOptions(
    options => { options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });
builder.Services.Configure<RouteOptions>(
    options => options.SetParameterPolicy<RegexInlineRouteConstraint>("regex"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddSqlServerDbContext<TechTestDbContext>("techTest");
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

var productsApi = app.MapGroup("/products");

productsApi.MapPost(
    "/new",
    async (HttpContext context, IProductRepository productRepository) =>
    {
        var product = await context.Request.ReadFromJsonAsync<Product>();
        if (product is null)
        {
            return Results.BadRequest();
        }

        var added = await productRepository.AddAsync(product);

        return added ? Results.Created() : Results.BadRequest();
    });

productsApi.MapGet(
    "/",
    async (IProductRepository productRepository) =>
    {
        return Results.Ok(await productRepository.GetAll());
    });

productsApi.MapGet(
    "/charts/quantity",
    async (IProductRepository productRepository) =>
    {
        return Results.Ok(await productRepository.GetByCategoryAsync());
    });

productsApi.MapGet(
    "/charts/timeframe",
    async (IProductRepository productRepository) =>
    {
        return Results.Ok(await productRepository.GetByDateAddedAsync());
    });

app.MapGet(
    "/categories",
    async (TechTestDbContext dbContext) => Results.Ok(await dbContext.Categories.ToListAsync()));

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days.
    // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Run();

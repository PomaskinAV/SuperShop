using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShopBackend.Repositories;
using OnlineShop.Data.EntityFramework;
using OnlineShop.Domain;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbPath = "myapp.db";
builder.Services.AddDbContext<AppDbContext>(
   options => options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>)); //исправить
builder.Services.AddScoped<IAccountRepository, AccountRepositoryEf>();
builder.Services.AddScoped<AccountService>();

var app = builder.Build();

app.UseCors(policy =>
policy.WithOrigins("https://localhost:5001")
.AllowAnyHeader()
.AllowAnyMethod()
);

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", (IAccountRepository repo) => { });
app.MapGet("/get_products", GetAllProducts);
app.MapGet("/get_product", GetProduct);

async Task<IResult> GetProduct(
	[FromQuery] Guid id,
	[FromServices] IRepository<Product> repository,
	CancellationToken cancellationToken)
{
	try
	{
        var product = await repository.GetById(id, cancellationToken);
        return Results.Ok(product);
    } catch (InvalidOperationException)
	{
        return Results.NotFound();
    }
}

app.MapPost("/add_product", AddProduct);
app.MapGet("/get_products/{id}", GetProductById);
app.MapPost("/update_product", UpdateProduct);
app.MapPost("/delete_product", DeleteProduct);

async Task<IResult> GetAllProducts(
	AppDbContext dbContext,
	CancellationToken cancellationToken,
	ILogger<Program> logger)
{
	try
	{
		logger.LogInformation("Запустили эндпоинт get_products");
		await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
		logger.LogInformation("Пытаемся получить товары...");
		var products = await dbContext.Products.ToArrayAsync(cancellationToken);
		return Results.Ok(products);
	} catch (OperationCanceledException)
	{
		logger.LogInformation("Операция отменена");
		return Results.NoContent();
	}
}

async Task AddProduct(Product product, AppDbContext dbContext)
{
	await dbContext.Products.AddAsync(product);
	await dbContext.SaveChangesAsync();
}

async Task<Product> GetProductById(Guid id, AppDbContext dbContext)
{
	var product = await dbContext.Products.FindAsync(id);

	if (product == null)
	{
		return null;
	}

	return product;
}

async Task UpdateProduct([FromQuery] Guid productId, [FromBody] Product updatedProduct, AppDbContext dbContext, HttpContext context)
{
    var product = await dbContext.Products.FindAsync(productId);
    if (product != null)
    {
        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        await dbContext.SaveChangesAsync();
        context.Response.StatusCode = StatusCodes.Status200OK;
    }
}

async Task DeleteProduct([FromQuery] Guid productId, AppDbContext dbContext, HttpContext context)
{
    var product = await dbContext.Products.FindAsync(productId);
    if (product != null)
    {
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();
        context.Response.StatusCode = StatusCodes.Status204NoContent;
    }
}

app.Run();

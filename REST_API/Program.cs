using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using REST_API.DataAccess;
using REST_API.Models;

namespace REST_API;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddControllers();
		builder.Services.AddDbContext<DataAccess.EntityFramework.ToDoContext>(options => options.UseInMemoryDatabase("ToDoList"));
		builder.Services.AddDbContext<DataAccess.EntityFramework.InvoicesDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Invoices")));

		// Old
		// builder.Services.AddDbContext<InvoicesDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		// builder.Services.AddEndpointsApiExplorer();
		// builder.Services.AddSwaggerGen();

		builder.Services.AddScoped<ICRUDBase<Category>, CategoriesDAO>();
		builder.Services.AddScoped<ICRUDBase<Invoice>, InvoicesDAO>();

		builder.Services.AddEndpointsApiExplorer();

		builder.Services.AddSwaggerGen();

		WebApplication app = builder.Build();

		if (app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/error-development");
		}
		else
		{
			app.UseExceptionHandler("/error");
		}

		app.UseSwagger();
		app.UseSwaggerUI();

		app.UseHttpsRedirection();

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}

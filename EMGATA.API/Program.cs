using EMGATA.API.Data;
using EMGATA.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SQLite DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity
builder.Services.AddIdentity<User, IdentityRole<int>>(options => {
		options.Password.RequireDigit = true;
		options.Password.RequireLowercase = true;
		options.Password.RequireUppercase = true;
		options.Password.RequireNonAlphanumeric = true;
		options.Password.RequiredLength = 6;
	})
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();


builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<SignInManager<User>>();



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

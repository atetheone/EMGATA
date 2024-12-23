using System.Text;
using EMGATA.API.Data;
using EMGATA.API.Models;
using EMGATA.API.Repositories;
using EMGATA.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "EMGATA API", Version = "v1" });
    
	// Add JWT Authentication configuration
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});

	// // Add file upload support
	// c.OperationFilter<FormFileOperationFilter>();
});

// Add SQLite DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireUppercase = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequiredLength = 6;
})
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();

// Disable email confirmation requirement
builder.Services.Configure<IdentityOptions>(options =>
{
	options.SignIn.RequireConfirmedEmail = false;
});

builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<SignInManager<User>>();

// Configurer l’authentification JWT
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
	};

});

// Register Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IModelService, ModelService>();
builder.Services.AddScoped<ICarImageService, CarImageService>();
builder.Services.AddScoped<IImageStorageService, ImageStorageService>();

// Register Repositories
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IModelRepository, ModelRepository>();
builder.Services.AddScoped<ICarImageRepository, CarImageRepository>();


// Add CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll",
		builder => builder
			.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader());
});

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Create roles and admin user
using (var scope = app.Services.CreateScope())
{
	var serviceProvider = scope.ServiceProvider;
	try
	{
		var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
		var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
		var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

		// Create roles if they don't exist
		string[] roles = { "Admin", "User" };
		foreach (var role in roles)
		{
			if (!await roleManager.RoleExistsAsync(role))
			{
				logger.LogInformation($"Creating role: {role}");
				await roleManager.CreateAsync(new IdentityRole<int>(role));
			}
		}

		// Create admin user if it doesn't exist
		var adminEmail = "admin@emgata.com";
		var adminPassword = "Admin@123";

		var adminUser = await userManager.FindByEmailAsync(adminEmail);

		if (adminUser == null)
		{
			logger.LogInformation("Creating admin user");
			var admin = new User
			{
				UserName = adminEmail, // Set username same as email for consistency
				Email = adminEmail,
				EmailConfirmed = true
			};

			var result = await userManager.CreateAsync(admin, adminPassword);

			if (result.Succeeded)
			{
				logger.LogInformation("Admin user created successfully");
				await userManager.AddToRoleAsync(admin, "Admin");
			}
			else
			{
				logger.LogError("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
			}
		}
		else
		{
			logger.LogInformation("Admin user already exists");
			// Ensure admin is in Admin role
			if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
			{
				await userManager.AddToRoleAsync(adminUser, "Admin");
			}
		}
	}
	catch (Exception ex)
	{
		var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "An error occurred while creating roles and admin user");
	}
}

app.Run();

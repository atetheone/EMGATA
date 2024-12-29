using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using EMGATA.API.Data;
using EMGATA.API.Models;
using EMGATA.API.Services;
using EMGATA.API.Models.Enums;
using EMGATA.API.Repositories;

namespace EMGATA.Tests.Integration;

[TestClass]
public class IntegrationTests : TestBase
{
  private IServiceProvider _serviceProvider;
  private IBrandService _brandService;
  private IModelService _modelService;
  private ICarService _carService;
  private UserManager<User> _userManager;
  private RoleManager<IdentityRole<int>> _roleManager;
  private ITokenService _tokenService;

  public IntegrationTests() : base()
  {
    var services = new ServiceCollection();

    // Add DbContext
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

    // Add Identity
    services.AddIdentity<User, IdentityRole<int>>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    // Add Identity Roles
    services.AddScoped<RoleManager<IdentityRole<int>>>();

    // Add Repositories
    services.AddScoped<IBrandRepository, BrandRepository>();
    services.AddScoped<IModelRepository, ModelRepository>();
    services.AddScoped<ICarRepository, CarRepository>();

    // Add Services
    services.AddScoped<IBrandService, BrandService>();
    services.AddScoped<IModelService, ModelService>();
    services.AddScoped<ICarService, CarService>();
    services.AddScoped<ITokenService, TokenService>();
    services.AddLogging();

    // Add Configuration
    services.AddSingleton(_configuration);

    _serviceProvider = services.BuildServiceProvider();

    // Get service instances
    _brandService = _serviceProvider.GetRequiredService<IBrandService>();
    _modelService = _serviceProvider.GetRequiredService<IModelService>();
    _carService = _serviceProvider.GetRequiredService<ICarService>();
    _userManager = _serviceProvider.GetRequiredService<UserManager<User>>();
    _roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    _tokenService = _serviceProvider.GetRequiredService<ITokenService>();
  }

  [TestInitialize]
  public async Task Setup()
  {
    // Create roles first
    string[] roles = { "Admin", "User" };
    foreach (var roleName in roles)
    {
      if (!await _roleManager.RoleExistsAsync(roleName))
      {
        await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
      }
    }

    // Then seed test data
    SeedTestData();
  }

  [TestCleanup]
  public void Cleanup()
  {
    ClearDatabase();
    if (_serviceProvider is IDisposable disposable)
    {
      disposable.Dispose();
    }
  }

  [TestMethod]
  public async Task FullCarSalesFlow_ShouldWorkCorrectly()
  {
    try
    {
      // Get initial count of available cars
      var initialAvailableCars = await _carService.GetAvailableCarsAsync();
      var initialCount = initialAvailableCars.Count();

      // Create a brand
      var brand = new Brand
      {
        Name = "Honda",
        Description = "Japanese manufacturer"
      };
      var createdBrand = await _brandService.CreateBrandAsync(brand);
      Assert.IsNotNull(createdBrand.Id, "Brand creation failed");

      // Create a model
      var model = new Model
      {
        BrandId = createdBrand.Id,
        Name = "Civic",
        Description = "Compact Sedan"
      };
      var createdModel = await _modelService.CreateModelAsync(model);
      Assert.IsNotNull(createdModel.Id, "Model creation failed");

      // Create a car
      var car = new Car
      {
        ModelId = createdModel.Id,
        UserId = 1, // Using the seeded test user
        Year = 2020,
        Color = "Silver",
        Price = 22000,
        Description = "New test car",
        Status = CarStatus.Available
      };
      var createdCar = await _carService.AddCarAsync(car);
      Assert.IsNotNull(createdCar.Id, "Car creation failed");

      // Verify count after adding new available car
      var availableCarsAfterAdd = await _carService.GetAvailableCarsAsync();
      Assert.AreEqual(initialCount + 1, availableCarsAfterAdd.Count(),
          "Available cars count mismatch after adding new car");

      // Update car status
      var updatedCar = await _carService.UpdateCarStatusAsync(createdCar.Id, CarStatus.Sold);
      Assert.AreEqual(CarStatus.Sold, updatedCar.Status, "Car status update failed");

      // Verify final count after marking as sold
      var finalAvailableCars = await _carService.GetAvailableCarsAsync();
      Assert.AreEqual(initialCount, finalAvailableCars.Count(),
          "Available cars count mismatch after status update");
    }
    catch (Exception ex)
    {
      Assert.Fail($"Test failed with exception: {ex.Message}");
    }
  }

  [TestMethod]
  public async Task AuthenticationFlow_ShouldWorkCorrectly()
  {
    try
    {
      // Verify roles exist
      Assert.IsTrue(await _roleManager.RoleExistsAsync("User"), "User role does not exist");

      // Register new user
      var user = new User
      {
        UserName = "newuser",
        Email = "newuser@example.com"
      };
      var password = "Test123!";
      var result = await _userManager.CreateAsync(user, password);
      Assert.IsTrue(result.Succeeded, "User creation failed");

      // Add to role
      var roleResult = await _userManager.AddToRoleAsync(user, "User");
      Assert.IsTrue(roleResult.Succeeded, "Role assignment failed");

      // Verify roles
      var roles = await _userManager.GetRolesAsync(user);
      Assert.IsTrue(roles.Contains("User"), "Role verification failed");

      // Generate token
      var token = _tokenService.GenerateToken(user, roles);
      Assert.IsNotNull(token, "Token generation failed");
      Assert.IsTrue(token.Length > 0, "Token is empty");

      // Verify user exists
      var foundUser = await _userManager.FindByEmailAsync(user.Email);
      Assert.IsNotNull(foundUser, "User not found");
      Assert.AreEqual(user.Email, foundUser.Email, "User email mismatch");

      // Verify password
      var isPasswordValid = await _userManager.CheckPasswordAsync(foundUser, password);
      Assert.IsTrue(isPasswordValid, "Password validation failed");
    }
    catch (Exception ex)
    {
      Assert.Fail($"Test failed with exception: {ex.Message}");
    }
  }
}
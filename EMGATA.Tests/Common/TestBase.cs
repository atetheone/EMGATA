// TestBase.cs
using Microsoft.EntityFrameworkCore;
using EMGATA.API.Data;
using Microsoft.Extensions.Configuration;
using EMGATA.API.Models;
using EMGATA.API.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace EMGATA.Tests;

public abstract class TestBase
{
  protected ApplicationDbContext _context;
  protected IConfiguration _configuration;

  protected TestBase()
  {
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

    _context = new ApplicationDbContext(options);

    // Setup configuration
    var configuration = new Dictionary<string, string>
        {
            {"Jwt:Key", "YourTestSecretKeyHereWhichShouldBeAtLeast32Chars"},
            {"Jwt:Issuer", "TestIssuer"},
            {"Jwt:Audience", "TestAudience"}
        };

    _configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(configuration)
        .Build();
  }

  protected void SeedTestData()
  {
    // Add brands
    var brands = new List<Brand>
        {
            new() { Id = 1, Name = "Toyota", Description = "Japanese car manufacturer" },
            new() { Id = 2, Name = "BMW", Description = "German car manufacturer" }
        };
    _context.Brands.AddRange(brands);

    // Add models
    var models = new List<Model>
        {
            new() { Id = 1, BrandId = 1, Name = "Camry", Description = "Sedan" },
            new() { Id = 2, BrandId = 1, Name = "RAV4", Description = "SUV" },
            new() { Id = 3, BrandId = 2, Name = "3 Series", Description = "Luxury sedan" }
        };
    _context.Models.AddRange(models);

    // Add users
    var users = new List<User>
        {
            new() { Id = 1, UserName = "testuser", Email = "test@example.com" }
        };
    _context.Users.AddRange(users);

    // Add cars
    var cars = new List<Car>
        {
            new() {
                Id = 1,
                ModelId = 1,
                UserId = 1,
                Year = 2020,
                Color = "Red",
                Price = 25000,
                Status = CarStatus.Available,
                Description = "Test car 1"
            },
            new() {
                Id = 2,
                ModelId = 2,
                UserId = 1,
                Year = 2021,
                Color = "Blue",
                Price = 30000,
                Status = CarStatus.Sold,
                Description = "Test car 2"
            }
        };
    _context.Cars.AddRange(cars);

    _context.SaveChanges();
  }

  protected void ClearDatabase()
  {
    _context.Cars.RemoveRange(_context.Cars);
    _context.Models.RemoveRange(_context.Models);
    _context.Brands.RemoveRange(_context.Brands);
    _context.Users.RemoveRange(_context.Users);
    _context.SaveChanges();
  }
}

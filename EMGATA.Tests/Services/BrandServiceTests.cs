using EMGATA.API.Services;
using EMGATA.API.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EMGATA.Tests.Services;

[TestClass]
public class BrandServiceTests : TestBase
{
  private IBrandService _brandService;
  private IBrandRepository _brandRepository;

  [TestInitialize]
  public void Setup()
  {
    _brandRepository = new BrandRepository(_context);
    _brandService = new BrandService(_brandRepository);
    SeedTestData();
  }

  [TestCleanup]
  public void Cleanup()
  {
    ClearDatabase();
  }

  [TestMethod]
  public async Task GetAllBrands_ShouldReturnAllBrands()
  {
    // Act
    var result = await _brandService.GetAllBrandsAsync();

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual(2, result.Count());
  }

  [TestMethod]
  public async Task GetBrandById_WithValidId_ShouldReturnBrand()
  {
    // Act
    var result = await _brandService.GetBrandByIdAsync(1);

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual("Toyota", result.Name);
  }

  [TestMethod]
  public async Task CreateBrand_ShouldAddNewBrand()
  {
    // Arrange
    var newBrand = new EMGATA.API.Models.Brand
    {
      Name = "Honda",
      Description = "Japanese manufacturer"
    };

    // Act
    var result = await _brandService.CreateBrandAsync(newBrand);

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual("Honda", result.Name);
    Assert.IsTrue(result.Id > 0);
  }
}

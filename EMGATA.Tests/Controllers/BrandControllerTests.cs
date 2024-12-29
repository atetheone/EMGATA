using AutoMapper;
using EMGATA.API.Controllers;
using EMGATA.API.Dtos;
using EMGATA.API.Models;
using EMGATA.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EMGATA.Tests.Controllers;

[TestClass]
public class BrandControllerTests
{
  private Mock<IBrandService> _mockBrandService;
  private Mock<IModelService> _mockModelService;
  private Mock<IMapper> _mockMapper;
  private BrandController _controller;

  [TestInitialize]
  public void Setup()
  {
    _mockBrandService = new Mock<IBrandService>();
    _mockModelService = new Mock<IModelService>();
    _mockMapper = new Mock<IMapper>();
    _controller = new BrandController(_mockBrandService.Object, _mockModelService.Object, _mockMapper.Object);
  }

  [TestMethod]
  public async Task GetBrands_ReturnsOkResultWithBrands()
  {
    // Arrange
    var brands = new List<Brand>
        {
            new() { Id = 1, Name = "Toyota" },
            new() { Id = 2, Name = "Honda" }
        };
    var brandDtos = new List<BrandDto>
        {
            new() { Id = 1, Name = "Toyota" },
            new() { Id = 2, Name = "Honda" }
        };

    _mockBrandService.Setup(x => x.GetAllBrandsAsync())
        .ReturnsAsync(brands);
    _mockMapper.Setup(x => x.Map<IEnumerable<BrandDto>>(brands))
        .Returns(brandDtos);

    // Act
    var result = await _controller.GetBrands();

    // Assert
    Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    var okResult = result.Result as OkObjectResult;
    var returnedBrands = okResult.Value as IEnumerable<BrandDto>;
    Assert.AreEqual(2, returnedBrands.Count());
  }

  [TestMethod]
  public async Task GetBrand_WithValidId_ReturnsOkResult()
  {
    // Arrange
    var brand = new Brand { Id = 1, Name = "Toyota" };
    var brandDto = new BrandDto { Id = 1, Name = "Toyota" };

    _mockBrandService.Setup(x => x.GetBrandByIdAsync(1))
        .ReturnsAsync(brand);
    _mockMapper.Setup(x => x.Map<BrandDto>(brand))
        .Returns(brandDto);

    // Act
    var result = await _controller.GetBrand(1);

    // Assert
    Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    var okResult = result.Result as OkObjectResult;
    var returnedBrand = okResult.Value as BrandDto;
    Assert.AreEqual("Toyota", returnedBrand.Name);
  }

  [TestMethod]
  public async Task CreateBrand_WithValidData_ReturnsCreatedResult()
  {
    // Arrange
    var createDto = new CreateBrandDto { Name = "Toyota" };
    var brand = new Brand { Id = 1, Name = "Toyota" };
    var brandDto = new BrandDto { Id = 1, Name = "Toyota" };

    _mockMapper.Setup(x => x.Map<Brand>(createDto))
        .Returns(brand);
    _mockBrandService.Setup(x => x.CreateBrandAsync(brand))
        .ReturnsAsync(brand);
    _mockMapper.Setup(x => x.Map<BrandDto>(brand))
        .Returns(brandDto);

    // Act
    var result = await _controller.CreateBrand(createDto);

    // Assert
    Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
    var createdResult = result.Result as CreatedAtActionResult;
    Assert.AreEqual("GetBrand", createdResult.ActionName);
    Assert.AreEqual(1, createdResult.RouteValues["id"]);
  }
}

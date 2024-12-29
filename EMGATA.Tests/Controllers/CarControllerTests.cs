using System.Security.Claims;
using AutoMapper;
using EMGATA.API.Controllers;
using EMGATA.API.Dtos;
using EMGATA.API.Models;
using EMGATA.API.Models.Enums;
using EMGATA.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

[TestClass]
public class CarControllerTests
{
  private Mock<ICarService> _mockCarService;
  private Mock<ICarImageService> _mockCarImageService;
  private Mock<IImageStorageService> _mockImageStorageService;
  private Mock<IMapper> _mockMapper;
  private CarController _controller;

  [TestInitialize]
  public void Setup()
  {
    _mockCarService = new Mock<ICarService>();
    _mockCarImageService = new Mock<ICarImageService>();
    _mockImageStorageService = new Mock<IImageStorageService>();
    _mockMapper = new Mock<IMapper>();
    _controller = new CarController(
        _mockCarService.Object,
        _mockCarImageService.Object,
        _mockImageStorageService.Object,
        _mockMapper.Object);

    // Simuler l'authentification
    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "Admin")
            };
    var identity = new ClaimsIdentity(claims, "TestAuthType");
    var claimsPrincipal = new ClaimsPrincipal(identity);

    _controller.ControllerContext = new ControllerContext
    {
      HttpContext = new DefaultHttpContext { User = claimsPrincipal }
    };
  }

  [TestMethod]
  public async Task GetAvailableCars_ReturnsOkResultWithCars()
  {
    // Arrange
    var cars = new List<Car>
            {
                new() { Id = 1, Status = CarStatus.Available },
                new() { Id = 2, Status = CarStatus.Available }
            };
    var carDtos = new List<CarDto>
            {
                new() { Id = 1 },
                new() { Id = 2 }
            };

    _mockCarService.Setup(x => x.GetAvailableCarsAsync())
        .ReturnsAsync(cars);
    _mockMapper.Setup(x => x.Map<IEnumerable<CarDto>>(cars))
        .Returns(carDtos);

    // Act
    var result = await _controller.GetAvailableCars();

    // Assert
    Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    var okResult = result.Result as OkObjectResult;
    var returnedCars = okResult.Value as IEnumerable<CarDto>;
    Assert.AreEqual(2, returnedCars.Count());
  }

  [TestMethod]
  public async Task CreateCar_WithValidData_ReturnsCreatedResult()
  {
    // Arrange
    var createDto = new CreateCarDto
    {
      ModelId = 1,
      Year = 2020,
      Color = "Red",
      Price = 25000,
      Description = "Test Car"
    };

    var mappedCar = new Car
    {
      ModelId = createDto.ModelId,
      Year = createDto.Year,
      Color = createDto.Color,
      Price = createDto.Price,
      Description = createDto.Description
    };

    var savedCar = new Car
    {
      Id = 1,
      ModelId = createDto.ModelId,
      UserId = 1,
      Status = CarStatus.Available,
      Year = createDto.Year,
      Color = createDto.Color,
      Price = createDto.Price,
      Description = createDto.Description
    };

    var carDto = new CarDto { Id = 1 };

    _mockMapper.Setup(x => x.Map<Car>(createDto))
        .Returns(mappedCar);
    _mockCarService.Setup(x => x.AddCarAsync(It.IsAny<Car>()))
        .ReturnsAsync(savedCar);
    _mockMapper.Setup(x => x.Map<CarDto>(savedCar))
        .Returns(carDto);

    // Act
    var result = await _controller.CreateCar(createDto);

    // Assert
    Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
    var createdResult = result.Result as CreatedAtActionResult;
    Assert.AreEqual("GetCar", createdResult.ActionName);
    Assert.AreEqual(1, createdResult.RouteValues["id"]);
  }

  [TestMethod]
  public async Task UpdateCarStatus_WithValidData_ReturnsOkResult()
  {
    // Arrange
    var car = new Car { Id = 1, Status = CarStatus.Available };
    var carDto = new CarDto { Id = 1 };

    _mockCarService.Setup(x => x.UpdateCarStatusAsync(1, CarStatus.Sold))
        .ReturnsAsync(car);
    _mockMapper.Setup(x => x.Map<CarDto>(car))
        .Returns(carDto);

    // Act
    var result = await _controller.UpdateCarStatus(1, CarStatus.Sold);

    // Assert
    Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
  }
}
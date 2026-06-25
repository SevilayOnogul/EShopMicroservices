using Moq;
using FluentAssertions;
using Xunit;
using Discount.Grpc.Services;
using Discount.Grpc;
using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Tests.Services;

public class DiscountServiceTests
{
    private DiscountContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<DiscountContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new DiscountContext(options);
    }

    [Fact]
    public async Task GetDiscount_WithValidProductName_ShouldReturnCoupon()
    {
        // 1. Arrange (Hazırlık)
        var productName = "Keyboard";
        var request = new GetDiscountRequest { ProductName = productName };

        using var dbContext = GetInMemoryDbContext();
        var mockLogger = new Mock<ILogger<DiscountService>>();

        var fakeCoupon = new Coupon
        {
            Id = 1,
            ProductName = productName,
            Amount = 200,
            Description = "Keyboard Discount"
        };
        dbContext.Coupons.Add(fakeCoupon);
        await dbContext.SaveChangesAsync();

        var service = new DiscountService(dbContext, mockLogger.Object);

        // 2. Act (Çalıştırma)
        var result = await service.GetDiscount(request, null!);

        // 3. Assert (Doğrulama)
        result.Should().NotBeNull();
        result.ProductName.Should().Be(productName);
        result.Amount.Should().Be(200);
    }

    [Fact]
    public async Task CreateDiscount_WithValidRequest_ShouldSaveToDatabaseAndReturnCouponModel()
    {
        // 1. Arrange (Hazırlık)
        using var dbContext = GetInMemoryDbContext();
        var mockLogger = new Mock<ILogger<DiscountService>>();

        var service = new DiscountService(dbContext, mockLogger.Object);

        // gRPC üzerinden gelecek olan sahte istek parametrelerini hazırlıyoruz
        var request = new CreateDiscountRequest
        {
            Coupon = new CouponModel
            {
                ProductName = "Laptop",
                Amount = 500,
                Description = "Laptop Super Discount"
            }
        };

        // 2. Act (Çalıştırma)
        var result = await service.CreateDiscount(request, null!);

        // 3. Assert (Doğrulama)
        result.Should().NotBeNull();
        result.ProductName.Should().Be("Laptop");
        result.Amount.Should().Be(500);

        // Ekstra Doğrulama: Hafızadaki veritabanını kontrol ediyoruz, gerçekten 1 kayıt eklenmiş mi?
        var dbCoupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == "Laptop");
        dbCoupon.Should().NotBeNull();
        dbCoupon!.Amount.Should().Be(500);
    }
}
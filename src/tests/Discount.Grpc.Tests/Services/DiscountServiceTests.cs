using Moq;
using FluentAssertions;
using Xunit;
using Discount.Grpc.Services;
using Discount.Grpc;
using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Grpc.Core;

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

    [Fact]
    public async Task UpdateDiscount_WithValidRequest_ShouldUpdateDatabaseAndReturnCouponModel()
    {
        // 1. Arrange (Hazırlık)
        using var dbContext = GetInMemoryDbContext();
        var mockLogger = new Mock<ILogger<DiscountService>>();

        // Güncellenecek eski kuponu veritabanına ekliyoruz
        var existingCoupon = new Coupon
        {
            Id = 2,
            ProductName = "Mouse",
            Amount = 50,
            Description = "Old Mouse Discount"
        };
        dbContext.Coupons.Add(existingCoupon);
        await dbContext.SaveChangesAsync();

        // 🔥 KRİTİK DOKUNUŞ: EF Core'un hafızasındaki takibi temizliyoruz.
        // Böylece servis içindeki Update metodu çakışma yaşamadan çalışabilecek.
        dbContext.ChangeTracker.Clear();

        var service = new DiscountService(dbContext, mockLogger.Object);

        // gRPC üzerinden gelecek güncel bilgileri içeren istek parametresi
        var request = new UpdateDiscountRequest
        {
            Coupon = new CouponModel
            {
                Id = 2,
                ProductName = "Mouse",
                Amount = 80,
                Description = "Updated Mouse Super Discount"
            }
        };

        // 2. Act (Çalıştırma)
        var result = await service.UpdateDiscount(request, null!);

        // 3. Assert (Doğrulama)
        result.Should().NotBeNull();
        result.Amount.Should().Be(80);

        // Ekstra Doğrulama: Veritabanına gidip gerçekten güncellenmiş mi diye bakıyoruz
        var dbCoupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.Id == 2);
        dbCoupon.Should().NotBeNull();
        dbCoupon!.Amount.Should().Be(80);
        dbCoupon.Description.Should().Be("Updated Mouse Super Discount");
    }

    [Fact]
    public async Task DeleteDiscount_WithValidProductName_ShouldDeleteFromDatabaseAndReturnSuccessTrue()
    {
        // 1. Arrange (Hazırlık)
        using var dbContext = GetInMemoryDbContext();
        var mockLogger = new Mock<ILogger<DiscountService>>();

        var productName = "Monitor";
        var existingCoupon = new Coupon
        {
            Id = 3,
            ProductName = productName,
            Amount = 150,
            Description = "Monitor Discount"
        };
        dbContext.Coupons.Add(existingCoupon);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();

        var service = new DiscountService(dbContext, mockLogger.Object);
        var request = new DeleteDiscountRequest { ProductName = productName };

        // 2. Act (Çalıştırma)
        var result = await service.DeleteDiscount(request, null!);

        // 3. Assert (Doğrulama)
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        // Veritabanından gerçekten silindi mi?
        var dbCoupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == productName);
        dbCoupon.Should().BeNull();
    }

    [Fact]
    public async Task DeleteDiscount_WithNonExistingProductName_ShouldThrowRpcExceptionWithNotFoundStatus()
    {
        // 1. Arrange (Hazırlık)
        using var dbContext = GetInMemoryDbContext();
        var mockLogger = new Mock<ILogger<DiscountService>>();

        var service = new DiscountService(dbContext, mockLogger.Object);
        var request = new DeleteDiscountRequest { ProductName = "NonExistingProduct" };

        // 2. Act & 3. Assert (Hata Fırlatma Doğrulaması)
        var act = async () => await service.DeleteDiscount(request, null!);

        // Metodun RpcException fırlatmasını ve durum kodunun NotFound (404) olmasını bekliyoruz
        var exception = await act.Should().ThrowAsync<RpcException>();
        exception.Which.StatusCode.Should().Be(StatusCode.NotFound);
    }
}
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.CreateOrder;
using Ordering.Infrastructure.Data; 

namespace Ordering.Application.Tests.Orders.Commands.CreateOrder;

public class CreateOrderHandlerTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateOrderAndReturnResult()
    {
        // 1. Arrange (Hazırlık)
        using var dbContext = GetInMemoryDbContext();
        var handler = new CreateOrderHandler(dbContext);

        // DDD nesnelerini oluşturacak sahte DTO yapısını hazırlıyoruz
        var addressDto = new AddressDto("Ahmet", "Yılmaz", "ahmet@test.com", "Kadıköy", "Turkey", "Istanbul", "34000");
        var paymentDto = new PaymentDto("Ahmet Yılmaz", "1234567890123456", "12/28", "123", 1);
        var orderItemDto = new OrderItemDto(Guid.NewGuid(), Guid.NewGuid(), 2, 500);

        var orderDto = new OrderDto(
    Id: Guid.NewGuid(),
    CustomerId: Guid.NewGuid(),
    OrderName: "Test_Order",
    ShippingAddress: addressDto,
    BillingAddress: addressDto,
    Payment: paymentDto,
    Status: Ordering.Domain.Enums.OrderStatus.Pending, 
    OrderItems: new List<OrderItemDto> { orderItemDto }
);

        var command = new CreateOrderCommand(orderDto);

        // 2. Act (Çalıştırma)
        var result = await handler.Handle(command, CancellationToken.None);

        // 3. Assert (Doğrulama)
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();

        
        var dbOrder = dbContext.Orders.Local
            .FirstOrDefault(x => x.Id == Ordering.Domain.ValueObjects.OrderId.Of(result.Id))
            ?? dbContext.Orders.ToList().FirstOrDefault(x => x.Id == Ordering.Domain.ValueObjects.OrderId.Of(result.Id));

        dbOrder.Should().NotBeNull();
        dbOrder!.OrderName.Value.Should().Be("Test_Order");
    }
}
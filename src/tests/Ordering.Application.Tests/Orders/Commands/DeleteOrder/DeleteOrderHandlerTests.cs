using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Orders.Commands.DeleteOrder;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using Xunit;

namespace Ordering.Application.Tests.Orders.Commands.DeleteOrder;

public class DeleteOrderHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingOrder_ShouldDeleteFromDatabaseAndReturnSuccess()
    {
        // 1. Arrange (Hazırlık) - Tamamen Mock Yapısı
        var mockDbContext = new Mock<IApplicationDbContext>();
        var mockDbSet = new Mock<DbSet<Order>>();

        var orderId = OrderId.Of(Guid.NewGuid());

        
        var fakeOrder = Order.Create(
            id: orderId,
            customerId: CustomerId.Of(Guid.NewGuid()),
            orderName: OrderName.Of("To Be Deleted"),
            shippingAddress: null!, // Mock kullandığımız için buralara null geçebiliriz
            billingAddress: null!,
            payment: null!
        );

        
        mockDbContext.Setup(x => x.Orders).Returns(mockDbSet.Object);

        mockDbSet.Setup(m => m.FindAsync(new object[] { orderId }, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(fakeOrder);


        var handler = new DeleteOrderHandler(mockDbContext.Object);
        var command = new DeleteOrderCommand(orderId.Value);

        // 2. Act (Çalıştırma)
        var result = await handler.Handle(command, CancellationToken.None);

        // 3. Assert (Doğrulama)
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        mockDbSet.Verify(x => x.Remove(It.IsAny<Order>()), Times.Once);
        mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
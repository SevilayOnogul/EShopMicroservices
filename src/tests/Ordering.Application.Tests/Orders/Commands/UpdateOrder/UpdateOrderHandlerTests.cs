using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.UpdateOrder;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Tests.Orders.Commands.UpdateOrder;

public class UpdateOrderHandlerTests
{
    [Fact]
    public async Task Handle_WithValidCommand_ShouldUpdateOrderAndReturnSuccess()
    {
        // 1. Arrange (Hazırlık)
        var mockDbContext = new Mock<IApplicationDbContext>();
        var mockDbSet=new Mock<DbSet<Order>>();
        var orderId=Guid.NewGuid();

        // Güncellenecek eski sahte sipariş nesnesi
        var fakeOrder = Order.Create(
            id: OrderId.Of(orderId),
            customerId: CustomerId.Of(Guid.NewGuid()),
            orderName: OrderName.Of("Old Order Name"),
            shippingAddress: null!,
            billingAddress: null!,
            payment: null!
        );

        mockDbContext.Setup(x=>x.Orders).Returns(mockDbSet.Object);

        // Handler içindeki arama metodunu (FindAsync veya FirstOrDefaultAsync vb.) mock'luyoruz
        mockDbSet.Setup(m => m.FindAsync(new object[] { OrderId.Of(orderId) }, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(fakeOrder);

        // Güncellenecek yeni verileri hazırlıyoruz
        var addressDto = new AddressDto("Ahmet", "Yılmaz", "ahmet@test.com", "Kadıköy", "Türkiye", "Istanbul", "34000");
        var paymentDto = new PaymentDto("Ahmet Yılmaz", "1234567890123456", "12/28", "123", 1);
        var orderItemDto = new OrderItemDto(orderId, Guid.NewGuid(), 3, 600);

        var updatedOrderDto = new OrderDto(
            Id: orderId,
            CustomerId: Guid.NewGuid(),
            OrderName: "Updated Order Name",
            ShippingAddress: addressDto,
            BillingAddress: addressDto,
            Payment: paymentDto,
            Status: OrderStatus.Pending,
            OrderItems: new List<OrderItemDto> { orderItemDto }
        );

        var command = new UpdateOrderCommand(updatedOrderDto);
        var handler = new UpdateOrderHandler(mockDbContext.Object);

        // 2. Act (Çalıştırma)
        var result = await handler.Handle(command, CancellationToken.None);

        // 3. Assert (Doğrulama)
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        // SaveChangesAsync metodunun tetiklendiğini doğruluyoruz
        mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

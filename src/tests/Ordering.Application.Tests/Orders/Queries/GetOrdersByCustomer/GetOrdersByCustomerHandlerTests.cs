using Moq;
using FluentAssertions;
using MockQueryable.Moq;
using Ordering.Application.Data;
using Ordering.Application.Orders.Queries.GetOrdersByCustomer; 
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using Xunit;

namespace Ordering.Application.Tests.Orders.Queries.GetOrdersByCustomer;

public class GetOrdersByCustomerHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingCustomerId_ShouldReturnMatchingOrders()
    {
        // 1. Arrange (Hazırlık)
        var mockDbContext = new Mock<IApplicationDbContext>();

        // Test edeceğimiz hedef müşteri ID'sini oluşturuyoruz
        var targetCustomerId = CustomerId.Of(Guid.NewGuid());
        var otherCustomerId = CustomerId.Of(Guid.NewGuid());

        
        var address = Address.Of("Ahmet", "Yılmaz", "ahmet@test.com", "Kadıköy", "Turkey", "Istanbul", "34000");
        var payment = Payment.Of("Ahmet Yılmaz", "1234567890123456", "12/28", "123", 1);

        var fakeOrdersList = new List<Order>
        {
            Order.Create(
                id: OrderId.Of(Guid.NewGuid()),
                customerId: targetCustomerId, 
                orderName: OrderName.Of("Target Order"),
                shippingAddress: address,
                billingAddress: address,
                payment: payment
            ),
            Order.Create(
                id: OrderId.Of(Guid.NewGuid()),
                customerId: otherCustomerId, 
                orderName: OrderName.Of("Other Order"),
                shippingAddress: address,
                billingAddress: address,
                payment: payment
            )
        };

        // Listemizi asenkron destekleyen Mock DbSet'e çevirip DbContext'e bağlıyoruz
        var mockDbSet = fakeOrdersList.BuildMockDbSet();
        mockDbContext.Setup(x => x.Orders).Returns(mockDbSet.Object);

        // Sorguyu hedef müşteri ID'siyle tetikliyoruz
        var query = new GetOrdersByCustomerQuery(targetCustomerId.Value); 
        var handler = new GetOrdersByCustomerHandler(mockDbContext.Object);

        // 2. Act (Çalıştırma)
        var result = await handler.Handle(query, CancellationToken.None);

        // 3. Assert (Doğrulama)
        result.Should().NotBeNull(); 
        result.Orders.Should().NotBeNull(); 

        // Sadece hedef müşteriye ait olan 1 sipariş dönmeli, diğeri elenmeli
        result.Orders.Should().HaveCount(1);
        result.Orders.First().CustomerId.Should().Be(targetCustomerId.Value);
    }
}
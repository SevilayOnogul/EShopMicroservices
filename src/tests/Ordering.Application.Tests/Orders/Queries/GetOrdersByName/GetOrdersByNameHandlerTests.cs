using Moq;
using FluentAssertions;
using MockQueryable.Moq;
using Ordering.Application.Data;
using Ordering.Application.Orders.Queries.GetOrdersByName; 
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using Xunit;

namespace Ordering.Application.Tests.Orders.Queries.GetOrdersByName;

public class GetOrdersByNameHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingOrderName_ShouldReturnMatchingOrders()
    {
        // 1. Arrange (Hazırlık)
        var mockDbContext = new Mock<IApplicationDbContext>();

        var address = Address.Of("Ahmet", "Yılmaz", "ahmet@test.com", "Kadıköy", "Turkey", "Istanbul", "34000");
        var payment = Payment.Of("Ahmet Yılmaz", "1234567890123456", "12/28", "123", 1);

        var targetOrderName = "Telefon";
        var fakeOrdersList = new List<Order>
        {
            Order.Create(
                id: OrderId.Of(Guid.NewGuid()),
                customerId: CustomerId.Of(Guid.NewGuid()),
                orderName: OrderName.Of(targetOrderName), 
                shippingAddress: address,
                billingAddress: address,
                payment: payment
            ),
            Order.Create(
                id: OrderId.Of(Guid.NewGuid()),
                customerId: CustomerId.Of(Guid.NewGuid()),
                orderName: OrderName.Of("TV"),
                shippingAddress: address,
                billingAddress: address,
                payment: payment
            )
        };

        // Listemizi asenkron sorguları destekleyen Mock DbSet'e çeviriyoruz
        var mockDbSet = fakeOrdersList.BuildMockDbSet();
        mockDbContext.Setup(x => x.Orders).Returns(mockDbSet.Object);

        // Sorguyu "Telefon" kelimesiyle tetikliyoruz
        var query = new GetOrdersByNameQuery(targetOrderName);
        var handler = new GetOrdersByNameHandler(mockDbContext.Object);

        // 2. Act (Çalıştırma)
        var result = await handler.Handle(query, CancellationToken.None);

        // 3. Assert (Doğrulama)
        result.Should().NotBeNull();
        result.Orders.Should().NotBeNull();

        // Gelen listede sadece 1 adet sipariş olmalı (Diğer sipariş elenmiş olmalı)
        result.Orders.Should().HaveCount(1);
        result.Orders.First().OrderName.Should().Be(targetOrderName);
    }
}
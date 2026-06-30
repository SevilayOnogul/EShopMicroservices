using Moq;
using FluentAssertions;
using MockQueryable.Moq; // Yeni eklediğimiz paket
using Ordering.Application.Data;
using Ordering.Application.Orders.Queries.GetOrders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using BuildingBlocks.Pagination;
using Xunit;

namespace Ordering.Application.Tests.Orders.Queries.GetOrders;

public class GetOrdersHandlerTests
{
    [Fact]
    public async Task Handle_WithValidPagination_ShouldReturnPaginatedOrders()
    {
        // 1. Arrange (Hazırlık)
        var mockDbContext = new Mock<IApplicationDbContext>();

        var orderId = OrderId.Of(Guid.NewGuid());
        var fakeOrdersList = new List<Order>
        {
            Order.Create(
                id: orderId,
                customerId: CustomerId.Of(Guid.NewGuid()),
                orderName: OrderName.Of("Alpha Order"),
                shippingAddress: null!,
                billingAddress: null!,
                payment: null!
            )
        };

        var mockDbSet = fakeOrdersList.BuildMockDbSet();

        // DbContext'e sahte setimizi bağlıyoruz
        mockDbContext.Setup(x => x.Orders).Returns(mockDbSet.Object);

        var paginationRequest = new PaginationRequest(PageIndex: 0, PageSize: 10);
        var query = new GetOrdersQuery(paginationRequest);
        var handler = new GetOrdersHandler(mockDbContext.Object);

        // 2. Act (Çalıştırma)
        var result = await handler.Handle(query, CancellationToken.None);

        // 3. Assert (Doğrulama)
        result.Should().NotBeNull();
        result.Orders.Should().NotBeNull();
        result.Orders.PageIndex.Should().Be(0);
        result.Orders.PageSize.Should().Be(10);
        result.Orders.Data.Should().NotBeEmpty();
    }
}
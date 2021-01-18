using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WiredBrainCoffee.CupOrderAdmin.Core.Model;
using WiredBrainCoffee.CupOrderAdmin.Core.Services.OrderCreation;
using System.Threading.Tasks;
using WiredBrainCoffee.CupOrderAdmin.Core.DataInterfaces;
using Moq;

namespace WiredBrainCoffee.CupOrderAdmin.Core.Tests.Services.OrderCreation
{
    [TestClass]
    public class OrderCreationServiceTests
    {

        [TestMethod]
        public async Task ShouldStoreCreatedOrderInOrderCreationResultAsync()
        {
            var orderRepoMock = new Mock<IOrderRepository>();
            orderRepoMock.Setup(x => x.SaveAsync(It.IsAny<Order>())).ReturnsAsync((Order x) => x);
            var coffeeRepoMock = new Mock<ICoffeeCupRepository>();

            var orderCreationService = new OrderCreationService(orderRepoMock.Object,coffeeRepoMock.Object);

            var customer = new Customer { Id = 99 };

            int noOfOrderCups = 2;
            var orderCreationResult =
            await orderCreationService.CreateOrderAsync(customer, noOfOrderCups);
            Assert.AreEqual(OrderCreationResultCode.Success, orderCreationResult.ResultCode);
            Assert.AreEqual(customer.Id, orderCreationResult.CreatedOrder.CustomerId);
            Assert.IsNotNull(orderCreationResult.CreatedOrder);

        }
    }
}

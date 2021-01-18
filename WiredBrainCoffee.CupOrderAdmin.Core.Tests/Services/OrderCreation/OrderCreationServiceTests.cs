using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WiredBrainCoffee.CupOrderAdmin.Core.Model;
using WiredBrainCoffee.CupOrderAdmin.Core.Services.OrderCreation;
using System.Threading.Tasks;

namespace WiredBrainCoffee.CupOrderAdmin.Core.Tests.Services.OrderCreation
{
    [TestClass]
    public class OrderCreationServiceTests
    {

        [TestMethod]
        public async Task ShouldStoreCreatedOrderInOrderCreationResultAsync()
        {
            var orderCreationService = new OrderCreationService(null, null);

            var customer = new Customer { Id = 99 };

            int noOfOrderCups = 2;
            var orderCreationResult =
            await orderCreationService.CreateOrderAsync(customer, noOfOrderCups);
            Assert.AreEqual(OrderCreationResultCode.Success, orderCreationResult.ResultCode);
            Assert.AreEqual(customer.Id, orderCreationResult.CreatedOrder.Id);
            Assert.IsNotNull(orderCreationResult.CreatedOrder);

        }
    }
}

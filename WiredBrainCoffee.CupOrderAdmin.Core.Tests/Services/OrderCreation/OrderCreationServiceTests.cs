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
        private OrderCreationService _orderCreationService;
        private int _noOfCupsInStock;

        [TestInitialize]
        public void testInitialize()
        {
            _noOfCupsInStock = 10;
            var orderRepoMock = new Mock<IOrderRepository>();
            orderRepoMock.Setup(x => x.SaveAsync(It.IsAny<Order>())).ReturnsAsync((Order x) => x);
            var coffeeRepoMock = new Mock<ICoffeeCupRepository>();
            coffeeRepoMock.Setup(x => x.GetCoffeeCupsInStockCountAsync()).ReturnsAsync(_noOfCupsInStock);

            _orderCreationService = new OrderCreationService(orderRepoMock.Object, coffeeRepoMock.Object);
        }
        [TestMethod]
        public async Task ShouldStoreCreatedOrderInOrderCreationResultAsync()
        {

            var customer = new Customer { Id = 99 };

            int noOfOrderCups = 1;
            var orderCreationResult =
            await _orderCreationService.CreateOrderAsync(customer, noOfOrderCups);
            Assert.AreEqual(OrderCreationResultCode.Success, orderCreationResult.ResultCode);
            Assert.AreEqual(customer.Id, orderCreationResult.CreatedOrder.CustomerId);
            Assert.IsNotNull(orderCreationResult.CreatedOrder);

        }
        [TestMethod]
        public async Task ShouldStoreRemainingCupsInStockInOrderOrderCreationResult()
        {
            var customer = new Customer();
            int noOfOrderCups = 3;
            var noOfCups = _noOfCupsInStock - noOfOrderCups;
            var orderCreationResult =
            await _orderCreationService.CreateOrderAsync(customer, noOfOrderCups);
            Assert.AreEqual(OrderCreationResultCode.Success, orderCreationResult.ResultCode);
            Assert.AreEqual(noOfCups, orderCreationResult.RemainingCupsInStock);
        }
        [TestMethod]
        public async Task ShouldReturnStockExceededResultIfNotEnoughCupsInStock()
        {
            var customer = new Customer();
            int noOfOrderCups = _noOfCupsInStock + 1;
            var orderCreationResult =
            await _orderCreationService.CreateOrderAsync(customer, noOfOrderCups);
            Assert.AreEqual(OrderCreationResultCode.StockExceeded, orderCreationResult.ResultCode);
            Assert.AreEqual(_noOfCupsInStock, orderCreationResult.RemainingCupsInStock);
        }
    }
}

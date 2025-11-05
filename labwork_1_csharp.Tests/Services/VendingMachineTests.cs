using labwork_1_csharp.Enums;
using labwork_1_csharp.Models;
using labwork_1_csharp.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace labwork_1_csharp.Tests.Services
{
    public class VendingMachineTests
    {
        private readonly VendingMachine _vendingMachine;

        public VendingMachineTests()
        {
            _vendingMachine = new VendingMachine();
        }

        [Fact]
        public void GetProducts_ReturnOnlyList()
        {
            var products = _vendingMachine.GetProducts();
            
            Assert.NotNull(products);
            Assert.True(products.Count >= 2);
        }

        [Fact]
        public void GetBalanсe_Initial_ShouldBeZero()
        {
            var balanсe = _vendingMachine.GetBalance();
            
            Assert.Equal(0m, balanсe);
        }
        
        [Fact]
        public void GetIncome_Initial_ShouldBeZero()
        {
            var income = _vendingMachine.GetIncome();
            
            Assert.Equal(0m, income);
        }
    }
}
using labwork_1_csharp.Models;
using Xunit;

namespace labwork_1_csharp.Tests.Models
{
    // xUnit
    public class CoinTests
    {
        [Fact]
        public void InsertCoin_WhenCoinInserted_ShouldIncreaseBalance()
        {
            // Arrange
            var coin = new Coin();
            decimal initialAmount = 5m;

            // Act
            coin.InsertCoin(initialAmount);

            // Assert
            Assert.Equal(initialAmount, coin.Balance);
        }

        [Fact]
        public void InsertCoin_ZeroAmount_ShouldNotChangeBalance()
        {
            var coin = new Coin();

            coin.InsertCoin(0m);
            
            Assert.Equal(0m, coin.Balance);
        }
        
        [Fact]
        public void CanPurchase_SufficientBalance_ReturnTrue()
        {
            var coin = new Coin();
            coin.InsertCoin(10m);
            decimal price = 5m;
            var result = coin.CanPurchase(price);
            
            Assert.True(result);
        }
        
        [Fact]
        public void  CanPurchase_InsufficientBalance_ReturnFalse()
        {
            var coin = new Coin();
            coin.InsertCoin(10m);
            decimal price = 15m;
            
            var result = coin.CanPurchase(price);
            
            Assert.False(result);
        }

        [Fact]
        public void Purchase_SufficientBalance_ReduceBalanceIncreaseIncome()
        {
            var coin = new Coin();
            coin.InsertCoin(15m);
            decimal price = 5m;
            
            var result = coin.Purchase(price);
            
            Assert.True(result.Success);
            Assert.Equal(10m, coin.Balance);
            Assert.Equal(5m, coin.Income);
        }

        [Fact]
        public void Purchase_InsufficientBalance_ShouldFailWithoutChanges()
        {
            var coin = new Coin();
            coin.InsertCoin(15m);
            decimal price = 25m;
            
            var result = coin.Purchase(price);
            
            Assert.False(result.Success);
            Assert.Equal(15m, coin.Balance);
            Assert.Equal(0m, coin.Income);
        }

        [Fact]
        public void ReturnCoin_ReturnAmountAndBalanceReset()
        {
            var coin = new Coin();
            decimal initialAmount = 5m;
            
            coin.InsertCoin(initialAmount);
            
            var returnAmount =  coin.ReturnCoin();
            
            Assert.Equal(5m, returnAmount);
            Assert.Equal(0m, coin.Balance);
        }

        [Fact]
        public void TakeCoin_ReturnIncomeAndIncomeReset()
        {
            var coin = new Coin();
            coin.InsertCoin(10m);
            coin.Purchase(5m);
            
            var income = coin.TakeCoin();
            
            Assert.Equal(5m, income);
            Assert.Equal(0m, coin.Income);
            Assert.Equal(5m, coin.Balance);
        }

        [Fact]
        public void Reset_ReturnIncomeResetAndBalanceReset()
        {
            var coin = new Coin();
            coin.InsertCoin(10m);
            coin.Purchase(5m);
            
            coin.Reset();
            
            Assert.Equal(0m, coin.Income);
            Assert.Equal(0m, coin.Balance);
        }
    }
}
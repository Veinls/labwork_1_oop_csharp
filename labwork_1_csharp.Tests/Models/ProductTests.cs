using labwork_1_csharp.Models;
using Xunit;

namespace labwork_1_csharp.Tests.Models
{
    public class ProductTests
    {
        [Fact]
        public void ProductConstructor_SetThePropertyCorrectly()
        {
            var product = new Product(1, "Product name", 10m, 5);
            
            Assert.Equal(1, product.Id);
            Assert.Equal("Product name", product.Name);
            Assert.Equal(10m, product.Price);
            Assert.Equal(5,  product.Quantity);
        }

        [Fact]
        public void ProductConstructor_ReturnToFormattedString()
        {
            var product = new Product(1, "Product name", 10m, 5);
            
            var result = product.ToString();
            
            Assert.Contains("1",  result);
            Assert.Contains("Product name", result);
            Assert.Contains("10", result);
            Assert.Contains("5", result);
        }
    }
}
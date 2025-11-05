using Xunit;

namespace labwork_1_csharp.Tests.Models
{
    public class SimpleTest
    {
        [Fact]
        public void SimpleTest_ShouldWork()
        {
            // Arrange
            int a = 5;
            int b = 3;

            // Act
            int result = a + b;

            // Assert
            Assert.Equal(8, result);
        }
    }
}

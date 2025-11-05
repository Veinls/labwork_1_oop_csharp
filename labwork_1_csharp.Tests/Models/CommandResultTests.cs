using labwork_1_csharp.Models;
using Xunit;

namespace labwork_1_csharp.Tests.Models
{
    public class CommandResultTests
    {
        [Fact]
        public void CommandResult_SetProperty()
        {
            var result = new CommandResult
            {
                Success = true,
                Message = "Message"
            };
            
            Assert.True(result.Success);
            Assert.Equal("Message", result.Message);
        }

        [Fact]
        public void CommandResult_AllowModificationOfProperties()
        {
            var result = new CommandResult
            {
                Success = false,
                Message = "Message 1"
            };
            
            result.Success = true;
            result.Message = "Message 2";
            
            Assert.True(result.Success);
            Assert.Equal("Message 2", result.Message);
        }
    }
}
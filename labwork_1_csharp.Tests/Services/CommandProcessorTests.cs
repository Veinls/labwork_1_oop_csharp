using labwork_1_csharp.Enums;
using labwork_1_csharp.Interfaces;
using labwork_1_csharp.Models;
using labwork_1_csharp.Services;
using Moq;
using Xunit;

namespace labwork_1_csharp.Tests.Services
{
    public class CommandProcessorTests
    {
        private readonly Mock<ISelectionMode> _mockModeManager;
        private readonly Mock<IVendingMachine> _mockVendingMachine;
        private readonly CommandProcessor _commandProcessor;
        
        public CommandProcessorTests()
        {
            _mockModeManager = new Mock<ISelectionMode>();
            _mockVendingMachine = new Mock<IVendingMachine>();
            _commandProcessor = new CommandProcessor(_mockModeManager.Object, _mockVendingMachine.Object);
        }

        [Fact]
        public void ExecuteCommand_NoPermission_ReturnError()
        {
            _mockModeManager.Setup(m => m.VerifyPermission(It.IsAny<WorkMode>())).Returns(false);
            _mockVendingMachine.Setup(m => m.GetCommandDisplay(It.IsAny<WorkMode>())).Returns("Command");
            _mockModeManager.Setup(m => m.GetModeDisplay(It.IsAny<WorkMode>())).Returns("Mode");
            _mockModeManager.Setup(m => m.CurrentMode).Returns(WorkMode.CustomMode);
            
            var result = _commandProcessor.ExecuteCommand(WorkMode.CViewProduct);
            
            Assert.False(result.Success);
            Assert.Contains("не доступна",  result.Message);

        }
    }
}
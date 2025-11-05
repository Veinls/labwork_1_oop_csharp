using labwork_1_csharp.Enums;
using labwork_1_csharp.Models;
using labwork_1_csharp.Services;
using Xunit;

namespace labwork_1_csharp.Tests.Services
{
    public class SelectionModeTests
    {
        private readonly SelectionMode _selectionMode;

        public SelectionModeTests()
        {
            _selectionMode = new SelectionMode();
        }

        [Fact]
        public void CurrentMode_Default_BeCustomMode()
        {
            Assert.Equal(WorkMode.CustomMode,  _selectionMode.CurrentMode);
        }
    }
}
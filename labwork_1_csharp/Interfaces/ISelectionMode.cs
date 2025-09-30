using labwork_1_csharp.Enums;
using labwork_1_csharp.Models;

namespace labwork_1_csharp.Interfaces
{
    public interface ISelectionMode
    {
        WorkMode CurrentMode { get;}
        CommandResult SetCurrentMode(WorkMode mode);
        bool VerifyPermission(WorkMode command);
        string GetModeDisplay(WorkMode mode);
    }
}
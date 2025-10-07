using labwork_1_csharp.Enums;
using labwork_1_csharp.Models;
namespace labwork_1_csharp.Interfaces
{
    public interface ICommandProcessor
    {
        CommandResult ExecuteCommand(WorkMode command);
    }
}
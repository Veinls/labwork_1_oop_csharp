using labwork_1_csharp.Enums;
using labwork_1_csharp.Interfaces;
using labwork_1_csharp.Models;

namespace labwork_1_csharp.Services
{
    public class SelectionMode : ISelectionMode
    {
        private WorkMode _currentMode = WorkMode.CustomMode;

        public WorkMode CurrentMode => _currentMode;

        public CommandResult SetCurrentMode(WorkMode newMode)
        {
            if (newMode == WorkMode.CustomMode || newMode == WorkMode.AdminMode)
            {
                _currentMode = newMode;
                return new CommandResult { Success = true, Message = $"Режим изменен на : {GetModeDisplay(newMode)}" };
            }

            return new CommandResult { Success = false, Message = "Ошибка выбора режима!" };
        }

        public bool VerifyPermission(WorkMode mode)
        {
            return _currentMode switch
            {
                WorkMode.CustomMode => VerifyPermissionCustomMode(mode),
                WorkMode.AdminMode => VerifyPermissionAdminMode(mode),
                _ => false
            };
        }

        private bool VerifyPermissionCustomMode(WorkMode command)
        {
            return command switch
            {
                WorkMode.CViewProduct => true,
                WorkMode.CInsertCoin => true,   
                WorkMode.CSelectionProduct => true,
                WorkMode.CReturnCoin => true,
                WorkMode.AdminMode => true,
                _ => false
            };
        }

        private bool VerifyPermissionAdminMode(WorkMode command)
        {
            return command switch
            {
                // 2 вариант
                WorkMode.AAddProduct or 
                WorkMode.ATakeCoin or
                WorkMode.CustomMode => true,
                _ => false
            };
        }

        
        /*public bool VerifyPermission(WorkMode command)
        {
            switch (_currentMode)
            {
                case WorkMode.CustomMode:
                    switch(command)
                    {
                        case WorkMode.CViewProduct:
                            return true;
                        case WorkMode.CInsertCoin:
                            return true;
                        case WorkMode.CSelectionProduct:
                            return true;
                        case WorkMode.CReturnCoin:
                            return true;
                        case WorkMode.AdminMode:
                            return true;
                        default:
                            return false;
                    }

                case WorkMode.AdminMode:
                    switch(command)
                    {
                        case WorkMode.AAddProduct:
                            return true; 
                        case WorkMode.ATakeCoin:
                            return true;
                        case WorkMode.CustomMode:
                            return true;
                        default:
                            return false;
                    }
                default:
                    return false;
            }
        }*/
        
        
        public string GetModeDisplay(WorkMode mode)
        {
            return mode switch
            {
                WorkMode.CustomMode => "Пользовательский режим",
                WorkMode.AdminMode => "Режим администратора",
                _ => mode.ToString()
            };
        }
    }
}

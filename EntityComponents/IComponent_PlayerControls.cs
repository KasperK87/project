namespace ResidentSurvivor{
    class IComponent_PlayerControls : SadConsole.Components.InputConsoleComponent{

        public override void ProcessMouse(SadConsole.IScreenObject obj, 
            SadConsole.Input.MouseScreenObjectState info, out bool flag){
                
            flag = false;
        }

        public override void ProcessKeyboard(SadConsole.IScreenObject obj, 
            SadConsole.Input.Keyboard info, out bool flag){
            
            flag = false;
        }

    }
}
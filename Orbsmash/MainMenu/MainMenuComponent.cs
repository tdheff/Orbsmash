using System.Collections.Generic;
using Handy.UI;
using Nez;
using Nez.UI;

namespace Orbsmash.MainMenu
{
    public enum EMainMenuActionType { ButtonPress }

    public class MainMenuData
    {
        public bool Play;
        public bool Quit;
    }

    public class MainMenuAction : UIAction<EMainMenuActionType, MainMenuData>
    {
        public MainMenuAction(EMainMenuActionType type, MainMenuData data) : base(type, data)
        {
        }

        public MainMenuAction(EMainMenuActionType type) : base(type)
        {
        }
        
        public static MainMenuAction ButtonClicked(MainMenuData data)
        {
            return new MainMenuAction(EMainMenuActionType.ButtonPress, data);
        }
    }
    
    public class MainMenuComponent : Component
    {
        public void ClickPlay(Button button)
        {
            Actions.Enqueue(MainMenuAction.ButtonClicked(new MainMenuData() { Play = true }));
        }
        
        public void ClickQuit(Button button)
        {
            Actions.Enqueue(MainMenuAction.ButtonClicked(new MainMenuData() { Quit = true }));
        }
        
        public readonly Queue<MainMenuAction> Actions = new Queue<MainMenuAction>();
    }
}
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Nez;
using Orbsmash.Game;
using Scene = Handy.Scene;

namespace Orbsmash.MainMenu
{
    public class MainMenuSystem : EntitySystem
    {
        public MainMenuSystem() : base(new Matcher().all(typeof(MainMenuComponent))) { }
        
        protected override void process(List<Entity> entities)
        {
            base.process(entities);
            
            foreach (var entity in entities)
            {
                var mainMenuComponent = entity.getComponent<MainMenuComponent>();

                while (mainMenuComponent.Actions.Count > 0)
                {
                    var action = mainMenuComponent.Actions.Dequeue();

                    switch (action.Type)
                    {
                        case EMainMenuActionType.ButtonPress:
                            if (action.Data.Play)
                            {
                           
                                Orbsmash.Get().ChangeScene(new Game.Game(new GameSettings(1)));
                                break;
                            } else if (action.Data.Quit)
                            {
                                Orbsmash.Get().Exit();
                            }
                            break;
                    }
                }
            }
        }
    }
}
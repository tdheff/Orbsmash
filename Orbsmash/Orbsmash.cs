using System.Runtime.InteropServices;
using System.Xml.Serialization.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Handy.Systems;
using Handy.Components;
using Handy.Dispatch;
using Nez;
using Nez.Sprites;
using Orbsmash.Game;
using Orbsmash.Player;
using Collider = Nez.Collider;
using Scene = Handy.Scene;

namespace Orbsmash
{
    /// <summary>
    /// The main class for the game of Orbsmash
    /// </summary>
    public class Orbsmash : Core
    {
        private static Orbsmash self;
        public QueueDispatcher Dispatcher = new QueueDispatcher();

        public Orbsmash() : base(windowTitle: "Orbsmash")
        {
            self = this;
        }

        public static Orbsmash Get()
        {
            return self;
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            Window.AllowUserResizing = true;
            debugRenderEnabled = true;

            var settings = new GameSettings();
            var gameScene = new Game.Game(settings);
            scene = gameScene;

            // set the scene so Nez can take over
            // scene = new MainMenu.MainMenu();
        }

        public void ChangeScene(Scene newScene)
        {
            scene = newScene;
        }
    }
}

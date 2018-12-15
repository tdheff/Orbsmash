using System.Runtime.InteropServices;
using System.Xml.Serialization.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Handy.Systems;
using Handy.Components;
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
        private Scene _scene;
        private Scene Scene
        {
            get => _scene;
            set {
                _scene = value;
                scene = value;
            }
        }

        private static Orbsmash self;

        public Orbsmash() : base(width: 2560, height: 1440, windowTitle: "Orbsmash")
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

            var settings = new GameSettings(numPlayers: 1);
            var gameScene = new Game.Game(settings);

            // set the scene so Nez can take over
            Scene = new MainMenu.MainMenu();
        }

        public void ChangeScene(Scene scene)
        {
            Scene = scene;
        }
    }
}

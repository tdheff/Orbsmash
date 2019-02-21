using Handy.Dispatch;
using Nez;
using Orbsmash.Game;
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
            //debugRenderEnabled = true;

            var settings = new GameSettings();
            var gameScene = new Game.Game(settings);
            // var gameScene = new PracticeGame();
            scene = gameScene;
            Screen.isFullscreen = true;

            // set the scene so Nez can take over
            // scene = new MainMenu.MainMenu();
        }

        public void ChangeScene(Scene newScene)
        {
            scene = newScene;
        }
    }
}

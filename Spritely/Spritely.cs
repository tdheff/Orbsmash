using Nez;

namespace Spritely
{
    /// <summary>
    /// The main class for the game of Spritely
    /// </summary>
    public class Spritely : Core
    {
        private static Spritely self;

        public Spritely() : base(windowTitle: "Spritely")
        {
            self = this;
        }

        public static Spritely Get()
        {
            return self;
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            Window.AllowUserResizing = true;
            debugRenderEnabled = true;

            var myScene = new MainView();
            scene = myScene;
        }

        public void ChangeScene(Scene newScene)
        {
            scene = newScene;
        }
    }
}

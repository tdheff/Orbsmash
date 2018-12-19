using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;

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

//            Texture2D texture = null;
//            var filePath = string.Empty;
//
//            using (OpenFileDialog openFileDialog = new OpenFileDialog())
//            {
//                openFileDialog.InitialDirectory = "c:\\";
//                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
//                openFileDialog.FilterIndex = 2;
//                openFileDialog.RestoreDirectory = true;
//
//                if (openFileDialog.ShowDialog() == DialogResult.OK)
//                {
//                    //Get the path of specified file
//                    filePath = openFileDialog.FileName;
//
//                    //Read the contents of the file into a stream
//                    var fileStream = openFileDialog.OpenFile();
//
//                    texture = Texture2D.FromStream(graphicsDevice, fileStream);
//                }
//            }
//
//            if (texture == null)
//            {
//                Exit();
//            }
//
//            var sprite = new Sprite(texture);
//            var entity = new Entity();
//            entity.addComponent(sprite);
//                
//            var myScene = Scene.createWithDefaultRenderer();
//            myScene.addEntity(entity);

            var myScene = new MainView();

            scene = myScene;
        }

        public void ChangeScene(Scene newScene)
        {
            scene = newScene;
        }
    }
}

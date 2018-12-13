using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;

namespace Orbsmash
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Orbsmash : Core
    {
        public Orbsmash() : base(width: 1280, height: 768, isFullScreen: false, enableEntitySystems: false)
        { }


        protected override void Initialize()
        {
            base.Initialize();
            Window.AllowUserResizing = true;

            // create our Scene with the DefaultRenderer and a clear color of CornflowerBlue
            var myScene = new Scene();
            var myRenderer = new RenderLayerRenderer(1, new int[] { 0, 1, 2 });
            myScene.clearColor = new Color(0.1f, 0.1f, 0.1f);
            myScene.addRenderer(myRenderer);

            // load a Texture. Note that the Texture is loaded via the scene.content class. This works just like the standard MonoGame Content class
            // with the big difference being that it is tied to a Scene. When the Scene is unloaded so too is all the content loaded via myScene.content.
            var texture = myScene.content.Load<Texture2D>(Nez.Content.player0);

            // setup our Scene by adding some Entities
            var entityOne = myScene.createEntity("entity-one");
            entityOne.addComponent(new Sprite(texture));

            var entityTwo = myScene.createEntity("entity-two");
            entityTwo.addComponent(new Sprite(texture));

            // move entityTwo to a new location so it isn't overlapping entityOne
            entityTwo.transform.position = new Vector2(200, 200);

            // set the scene so Nez can take over
            scene = myScene;
        }
    }
}

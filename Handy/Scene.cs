using Microsoft.Xna.Framework;
using Nez;

namespace Handy
{
    public abstract class Scene : Nez.Scene
    {   
        protected Scene() : base()
        {
            _initialize();
        }
        
        private void _initialize()
        {
            SetupRenderer();
            InstallSystems(Systems());
        }

        protected abstract EntitySystem[] Systems();
        
        protected void InstallSystems(params EntitySystem[] systems)
        {
            foreach (var system in systems)
            {
                addEntityProcessor(system);
            }
        }
        
        protected virtual void SetupRenderer()
        {
            var myRenderer = new RenderLayerRenderer(1, new int[] { 0 });
            clearColor = new Color(0.3f, 0.3f, 0.3f);
            addRenderer(myRenderer);
        }

    }
}
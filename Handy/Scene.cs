using Microsoft.Xna.Framework;
using Nez;

namespace Handy
{
    public abstract class Scene : Nez.Scene
    {
        private Vector2 _scale;
        
        protected Scene() : base()
        {
            _scale = Vector2.One;
            _initialize();
        }
        
        /// <summary>
        /// Constructs a scene with a scale factor
        /// </summary>
        /// <param name="scale">All entities added to the scene will be scaled by this amount</param>
        protected Scene(int scale) : base()
        {
            _scale = new Vector2(scale);
            _initialize();
        }

        public new Entity addEntity(Entity entity)
        {
            entity.scale = _scale;
            return base.addEntity(entity);
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
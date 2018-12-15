using Handy.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Orbsmash.Player;
using Scene = Handy.Scene;

namespace Orbsmash.Game
{
    /// <summary>
    /// A scene representing an actual instance of the game, once play has been selected in the menu
    /// </summary>
    public class Game : Scene
    {
        private readonly GameSettings _settings;
        
        public Game(GameSettings settings)
        {
            _settings = settings;
            CreateEntities();
        }
        
        protected void SetupRendering()
        {
            var gameRenderer = new RenderLayerRenderer(1, new int[] { 0 });
            clearColor = new Color(0.1f, 0.1f, 0.1f);
            var uiRenderer = new ScreenSpaceRenderer(1, new [] { 1 });
            addRenderer(gameRenderer);
        }

        protected override EntitySystem[] Systems()
        {
            return new EntitySystem[]
            {
                new PlayerInputSystem(),
                new PlayerMovementSystem(),
                new KinematicSystem(),
                new AnimationSystem()
            };
        }
        
        private void CreateEntities()
        {
            var texture = content.Load<Texture2D>(Nez.Content.player0);
            for (var i = 0; i < _settings.NumPlayers; i++)
            {
                var player = new Player.Player(i, texture, i % 2 == 0 ? Constants.Side.Left : Constants.Side.Right);
                addEntity(player);
            }
            
            var ball = new Ball.Ball(texture);
            addEntity(ball);
        }
    }
}
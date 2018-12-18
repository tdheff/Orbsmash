using Handy.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Tiled;
using Orbsmash.Player;
using TiledSharp;
using Scene = Handy.Scene;

namespace Orbsmash.Game
{
    /// <summary>
    /// A scene representing an actual instance of the game, once play has been selected in the menu
    /// </summary>
    public class Game : Scene
    {
        private readonly GameSettings _settings;
        
        public Game(GameSettings settings) : base(4)
        {
            setDesignResolution(1920, 1080, SceneResolutionPolicy.BestFit);
            _settings = settings;
            CreateEntities();
        }
        
        protected void SetupRendering()
        {
            //var gameRenderer = new RenderLayerRenderer(1, new int[] { 0, 1, 2, 3 });
            var gameRenderer = new DefaultRenderer();
            clearColor = new Color(0.1f, 0.1f, 0.1f);
            addRenderer(gameRenderer);
        }

        protected override EntitySystem[] Systems()
        {
            return new EntitySystem[]
            {
                new PlayerInputSystem(),
                new PlayerStateMachineSystem(),
                new PlayerMovementSystem(),
                new KinematicSystem(),
                new AnimationSystem()
            };
        }
        
        private void CreateEntities()
        {
            var tiledMap = content.Load<TiledMap>("Tiles/DungeonMap");
            var tiledMapComponent = new TiledMapComponent(tiledMap, "Colliders", true);
            var entity = new Entity();
            entity.name = "Map";
            entity.addComponent(tiledMapComponent);
            addEntity(entity);
            
            var texture = content.Load<Texture2D>("Sprites/Characters/Pirate/Pirate");
            for (var i = 0; i < _settings.NumPlayers; i++)
            {
                var player = new Player.Player(i, texture, i % 2 == 0 ? Constants.Side.Left : Constants.Side.Right);
                addEntity(player);
            }
            
            var ball = new Ball.Ball(texture);
            addEntity(ball);
        }

        public override void onStart()
        {
            findEntity("Map").transform.position = new Vector2(-32 * 4, 0);
            findEntity("Player_0").transform.position = new Vector2(200, 200);
            findEntity("Ball").transform.position = new Vector2(400, 400);
        }
    }
}
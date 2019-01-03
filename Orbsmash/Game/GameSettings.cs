using System.Collections.Generic;
using Orbsmash.Constants;
namespace Orbsmash.Game
{

    public class PlayerSettings
    {
        public int Id;
        public float Speed;
        public string Sprite;
        public string Hitbox;
        public Gameplay.Side Side;
    }
    public class GameSettings
    {
        public List<PlayerSettings> Players = new List<PlayerSettings>();
        public string BallSprite;
        public string MapTile;
        public int NumPlayers => Players.Count;

        public GameSettings(bool testing = true)
        {
            if(testing)
            {
                Players.Add(new PlayerSettings() { Id = 0, Sprite = PlayerSprites.KNIGHT, Side = Gameplay.Side.LEFT, Speed = 350f, Hitbox = Hitboxes.KNIGHT_HITBOX });
                BallSprite = BallSprites.DEFAULT;
                MapTile = MapTiles.DUNGEON;
            }
        }
    }
}
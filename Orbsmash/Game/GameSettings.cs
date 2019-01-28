using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Orbsmash.Constants;
using Orbsmash.Player;

namespace Orbsmash.Game
{

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
                Players.Add(new PlayerSettings
                {
                    Id = 0,
                    Side = Gameplay.Side.LEFT,
                    Speed = 500f,
                    StartingPosition = new Vector2(700, 700),
                    Character = Gameplay.Character.WIZARD
                });
                Players.Add(new PlayerSettings
                {
                    Id = 0,
                    Side = Gameplay.Side.RIGHT,
                    Speed = 500f,
                    StartingPosition = new Vector2(1400, 700),
                    Character = Gameplay.Character.WIZARD
                });
                BallSprite = BallSprites.DEFAULT;
                MapTile = MapTiles.MEDIEVAL;
            }
        }
    }
}
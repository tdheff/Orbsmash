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
        public float MasterVolume;
        public float MusicVolume;
        public float SfxVolume;

        public GameSettings(bool testing = true)
        {
            if(testing)
            {
                Players.Add(new PlayerSettings
                {
                    Id = 0,
                    Side = Gameplay.Side.LEFT,
                    Speed = 300f,
                    StartingPosition = new Vector2(575, 450),
                    Character = Gameplay.Character.KNIGHT
                });
                
                Players.Add(new PlayerSettings
                {
                    Id = 1,
                    Side = Gameplay.Side.RIGHT,
                    Speed = 300f,
                    StartingPosition = new Vector2(1345, 450),
                    Character = Gameplay.Character.KNIGHT
                });
                
                BallSprite = BallSprites.DEFAULT;
                MapTile = MapTiles.MEDIEVAL2;
                MasterVolume = 1;
                MusicVolume = .3f;
                SfxVolume = 1;
            }
        }
    }
}
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
        private float _MusicVolume;
        public float MusicVolume
        {
            get
            {
                return _MusicVolume;
            }
            set
            {
                _MusicVolume = MasterVolume * value;
            }
        }
        private float _SfxVolume;
        public float SfxVolume
        {
            get
            {
                return _SfxVolume;
            }
            set
            {
                _SfxVolume = MasterVolume * value;
            }
        }

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
                    Character = Gameplay.Character.WIZARD
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
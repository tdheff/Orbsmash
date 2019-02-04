using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbsmash.Player
{
    public class PlayerSoundStateComponent: Component
    {
        // for now we can use simple flags, later we can do fancy shit
        public bool PlayBallHit = false;
        public bool PlaySwingWeapon1 = false;
        public void ClearFlags()
        {
            PlayBallHit = false;
            PlaySwingWeapon1 = false;
        }
    }
}

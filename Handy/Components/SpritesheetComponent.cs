using System.Collections.Generic;
using System.Linq;
using Nez.Sprites;
using Nez.Textures;

namespace Handy.Components
{
    public class SpritesheetComponent : Sprite
    {
        public SpritesheetComponent(IEnumerable<Subtexture> subtextures) : base(subtextures.ToArray()[0])
        {
            Subtextures = subtextures.ToArray();
            _frame = 0;
        }
        
        public Subtexture[] Subtextures;
        private int _frame;

        public int Frame
        {
            get => _frame;
            set
            {
                _frame = value;
                subtexture = Subtextures[_frame];
            }
        }
    }
}
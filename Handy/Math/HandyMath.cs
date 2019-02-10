using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handy
{
    public static class HandyMath
    {
        public static float DegreesToRadians(float degrees)
        {
            return degrees * (float)Math.PI / 180;
        }
        public static float RadiansToDegrees(float radians)
        {
            return radians * 180 / (float) Math.PI;
        }

        public static Vector2 MakeNormalizedVectorFromAngleDegrees(float degrees)
        {
            var rads = DegreesToRadians(degrees);
            return new Vector2((float) Math.Cos(rads), (float) Math.Sin(rads));
        }
    }
}

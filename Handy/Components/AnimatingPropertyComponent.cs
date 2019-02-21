using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;

namespace Handy.Components
{
    // linear = A -> B
    // Sinusoidal = A * sin(t) where b = % of the sin wave you want the animation to go through
    // CircleDegrees = starting angle A, ending angle B, most direct path, angle in degrees
    public enum AnimationSchemes { Linear, Sinusoidal, CircleDegrees }

    public class AnimatingPropertyComponent : Component
    {
        private bool Repeat = false;
        private float AnimationLength;
        private AnimationSchemes Scheme;
        private float A;
        private float B;
        private float StartTime = 0;
        private float PauseTime;
        public AnimatingPropertyComponent() { }
        public AnimatingPropertyComponent(float a, float b, float length, AnimationSchemes scheme, bool repeat = false)
        {
            SetParameters(a, b, length, scheme, repeat);
        }

        public void Start()
        {
            StartTime = Time.time;
        }

        // not sure yet what kinda controls we want...
        public void Pause()
        {
            PauseTime = Time.time;
        }

        public void SetParameters(float a, float b, float length, AnimationSchemes scheme, bool repeat)
        {
            A = a;
            B = b;
            AnimationLength = length;
            Repeat = repeat;
            Scheme = scheme;
        }

        public float GetCurrentValue()
        {
            var t = Time.time - StartTime;
            // if we aren't repeating and we've gone past the end
            if(StartTime == 0)
            {
                switch (Scheme)
                {
                    case AnimationSchemes.Linear:
                        return A;
                    case AnimationSchemes.Sinusoidal:
                        return A;
                    case AnimationSchemes.CircleDegrees:
                        return A;
                    default:
                        return LinearImplementation(t);
                }
            }
            if(!Repeat && t > AnimationLength)
            {
                switch (Scheme)
                {
                    case AnimationSchemes.Linear:
                        return B;
                    case AnimationSchemes.Sinusoidal:
                        return A;
                    case AnimationSchemes.CircleDegrees:
                        return B;
                    default:
                        return LinearImplementation(t);
                }
            }
            else if(Repeat)
            {
                t = t % AnimationLength; // take remainder as t
            }
            // core implementation logic
            switch(Scheme)
            {
                case AnimationSchemes.Linear:
                    return LinearImplementation(t);
                case AnimationSchemes.Sinusoidal:
                    return SinusoidalImplementation(t);
                case AnimationSchemes.CircleDegrees:
                    return CircleDegreesImplementation(t);
                default:
                    return LinearImplementation(t);
            }
        }

        private float LinearImplementation(float t)
        {
            return A + (B - A) * (t / AnimationLength);
        }
        private float SinusoidalImplementation(float t)
        {
            // spread it out over 2pi and use B to % it
            var multiplier = (t / AnimationLength) * B;
            var inputToSin = Math.PI * 2 * multiplier;
            var sinResult = Math.Sin(inputToSin);
            return A * (float) sinResult;
        }
        private float CircleDegreesImplementation(float t)
        {
            var clockwise = false;
            float diff;
            if(A - B > 0)
            {
                if(A - B > 180)
                {
                    // we go counter clockwise
                    diff = 360 - (A - B);
                } else
                {
                    diff = A - B;
                    clockwise = true;
                }
            } else
            {
                if(B - A > 180)
                {
                    diff = 360 - (B - A);
                    clockwise = true;
                } else
                {
                    diff = B - A;
                    // we go counter clockwise
                }
            }
            float result;
            if (clockwise)
            {
                // means we are subtracting
                result = A - diff * (t/AnimationLength);

            } else
            {
                // we are adding
                result = A + diff * (t / AnimationLength);
            }
            // Console.WriteLine($"Circle Value: {result % 360f}; A: {A}, B: {B}");
            return result % 360f;
        }
    }
}

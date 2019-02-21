using Handy.Components;
using Orbsmash.Constants;
using Microsoft.Xna.Framework;

namespace Orbsmash.Player
{
    public enum WizardStates { Idle, Walk, Glide, Immaterial, Attack, Dead }
    
    public class WizardState : IStateMachineState<WizardStates>
    {
        // STATE
        public WizardStates StateEnum { get; set; } = WizardStates.Idle;

        // GLIDE
        public Vector2 GlideDirection = new Vector2(1, 0);
        public const float MAX_GLIDE_TIME = 0.4f;
        public const float GLIDE_COOLDOWN = 2.0f;
        public const float GLIDE_SPEED = 1000.0f;
        public float GlideTime = 0;
        public float LastGlideTime = 0;
        public float GlideCooldown = GLIDE_COOLDOWN;
        
        // IMMATERIAL
        public const float IMMATERIAL_TIME = 0.25f;
        public const float IMMATERIAL_COOLDOWN = 2.0f;
        public const float IMMATERIAL_MIN_HIT_BOOST = 1.3f;
        public const float IMMATERIAL_MAX_HIT_BOOST = 2.5f;
        public const float IMMATERIAL_BOOST_RANGE = IMMATERIAL_MAX_HIT_BOOST - IMMATERIAL_MIN_HIT_BOOST;
        public float ImmaterialTime = 0;
        public float ImmaterialCooldown = IMMATERIAL_COOLDOWN;

        public IStateMachineState<WizardStates> Clone()
        {
            return MemberwiseClone() as IStateMachineState<WizardStates>;
        }
    }
}
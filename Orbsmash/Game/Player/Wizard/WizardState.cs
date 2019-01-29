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
        public const float GLIDE_SPEED = 700.0f;
        public float GlideTime = 0;
        public float GlideCooldown = 0;
        
        // IMMATERIAL
        public const float IMMATERIAL_TIME = 0.2f;
        public const float IMMATERIAL_COOLDOWN = 2.0f;
        public const float IMMATERIAL_HIT_BOOST = 2.0f;
        public float ImmaterialTime = 0;
        public float ImmaterialCooldown = 0;

        public IStateMachineState<WizardStates> Clone()
        {
            return MemberwiseClone() as IStateMachineState<WizardStates>;
        }
    }
}
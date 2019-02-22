using Microsoft.Xna.Framework;
using Nez;
using Orbsmash.Constants;

namespace Orbsmash.Player
{
    public class PlayerStateComponent : Component
    {
        // CONSTANTS
        public const float MOVEMENT_THRESHOLD_SQUARED = 0.01f;
        
        // STATE
        // we don't actually update a single global state enum....
        public bool IsKilled = false;
        public bool HasKOBounced = false;
        
        // IDENTIFICATION
        public int playerId { get; private set; }
        public Gameplay.Side side { get; private set; }
        
        // MOVEMENT
        public bool CanMove = true;
        public bool DashFinished = false;
        public Vector2 LastVector = new Vector2();
        public Gameplay.Direction LastDirection;
        public Vector2 ResetPosition;

        // SWINGING
        public float ChargeTime;
        public const float MAX_CHARGE_TIME = 2.0f;
        public bool ChargeFinished = false;
        public bool SwingFinished = false;
        public bool HitActive = false;
        public Vector2 BallHitVector = new Vector2(0, 0);
        public float BallHitBoost = 1.0f;
        
        // OTHER
        public bool IsInvulnerable = false;
        
        public PlayerStateComponent(int playerId, Gameplay.Side side, float speed, Vector2 resetPosition)
        {
            this.playerId = playerId;
            this.side = side;
            ResetPosition = resetPosition;
        }
    }
}
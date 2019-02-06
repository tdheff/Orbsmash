using System.Collections.Generic;
using Handy.Components;
using Microsoft.Xna.Framework;
using Nez;
using Random = Nez.Random;

namespace Handy.Systems
{
    public class CameraShakeSystem : EntitySystem
    {
        public CameraShakeSystem() : base(new Matcher().all(typeof(Camera), typeof(CameraShakeComponent))) { }

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var camera = entity.getComponent<Camera>();
                var cameraShake = entity.getComponent<CameraShakeComponent>();

                if (cameraShake.RemainingDuration > 0)
                {
                    var angle = Random.nextAngle();
                    var amountRemaining = cameraShake.RemainingDuration / cameraShake.Duration;
                    var radius = Random.nextFloat() * cameraShake.Intensity * amountRemaining;

                    var x = radius * Mathf.cos(angle);
                    var y = radius * Mathf.sin(angle);
                    camera.position = new Vector2(x, y);

                    cameraShake.RemainingDuration -= Time.deltaTime;
                }
            }
        }
    }
}
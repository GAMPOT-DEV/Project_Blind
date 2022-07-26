using UnityEngine;

namespace Blind
{
    public class PlayerCharacterController2D: CharacterController2D
    {
        public void MakePlatformFallthrough()
        {
            int colliderCount = 0;
            int fallthroughColliderCount = 0;
        
            for (int i = 0; i < GroundColliders.Length; i++)
            {
                Collider2D col = GroundColliders[i];
                if(col == null)
                    continue;

                colliderCount++;

                if (PhysicHelper.ColliderHasPlatformEffector (col))
                    fallthroughColliderCount++;
            }

            if (fallthroughColliderCount == colliderCount)
            {
                for (int i = 0; i < GroundColliders.Length; i++)
                {
                    Collider2D col = GroundColliders[i];
                    if (col == null)
                        continue;

                    PlatformEffector2D effector;
                    PhysicHelper.TryGetPlatformEffector (col, out effector);
                    FallthroughReseter reseter = effector.gameObject.AddComponent<FallthroughReseter>();
                    reseter.StarFall(effector);
                }
            }
        }
    }
}
using UnityEngine;

namespace MinigameArena
{
    /// <summary>
    /// STRATEGY concreta: persegue o player em linha reta.
    /// Usada pelo inimigo comum (Grunt).
    /// </summary>
    public class ChaseStrategy : IMovementStrategy
    {
        public string Name => "Perseguir";

        public void Move(Transform self, Transform target, float speed, float deltaTime)
        {
            if (target == null) return;

            Vector3 dir = target.position - self.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
            {
                dir.Normalize();
                self.position += dir * speed * deltaTime;
                self.rotation = Quaternion.Slerp(self.rotation, Quaternion.LookRotation(dir), 10f * deltaTime);
            }
        }
    }
}

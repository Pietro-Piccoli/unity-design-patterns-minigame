using UnityEngine;

namespace MinigameArena
{
    /// <summary>
    /// STRATEGY concreta: vai em direcao ao player mas balancando em ziguezague,
    /// o que faz ele ser mais dificil de acertar. Usada pelo Runner (rapido).
    /// Guarda estado proprio (timer), por isso a Factory cria UMA instancia por inimigo.
    /// </summary>
    public class ZigZagStrategy : IMovementStrategy
    {
        public string Name => "Ziguezague";

        private float _timer;
        private readonly float _frequency;
        private readonly float _amplitude;

        public ZigZagStrategy(float frequency = 6f, float amplitude = 0.9f)
        {
            _frequency = frequency;
            _amplitude = amplitude;
        }

        public void Move(Transform self, Transform target, float speed, float deltaTime)
        {
            if (target == null) return;

            _timer += deltaTime;

            Vector3 dir = target.position - self.position;
            dir.y = 0f;
            if (dir.sqrMagnitude <= 0.001f) return;
            dir.Normalize();

            // vetor perpendicular pra criar o balanco lateral
            Vector3 perp = Vector3.Cross(Vector3.up, dir);
            Vector3 move = (dir + perp * Mathf.Sin(_timer * _frequency) * _amplitude).normalized;

            self.position += move * speed * deltaTime;
            self.rotation = Quaternion.Slerp(self.rotation, Quaternion.LookRotation(move), 10f * deltaTime);
        }
    }
}

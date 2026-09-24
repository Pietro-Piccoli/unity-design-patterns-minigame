using UnityEngine;

namespace MinigameArena
{
    /// <summary>
    /// STRATEGY concreta: fica parado "carregando" e depois da uma investida rapida
    /// na direcao que o player estava. Usada pelo Tank (forte e pesado).
    /// Tem uma pequena maquina de estados interna (carregando -> investindo).
    /// </summary>
    public class ChargeStrategy : IMovementStrategy
    {
        public string Name => "Investida";

        private enum State { WindUp, Dashing }
        private State _state = State.WindUp;

        private float _timer;
        private readonly float _windUpTime;
        private readonly float _dashTime;
        private readonly float _dashMultiplier;
        private Vector3 _dashDir;

        public ChargeStrategy(float windUpTime = 0.8f, float dashTime = 0.55f, float dashMultiplier = 4f)
        {
            _windUpTime = windUpTime;
            _dashTime = dashTime;
            _dashMultiplier = dashMultiplier;
        }

        public void Move(Transform self, Transform target, float speed, float deltaTime)
        {
            if (target == null) return;

            _timer += deltaTime;

            if (_state == State.WindUp)
            {
                // mira no player enquanto carrega
                Vector3 look = target.position - self.position;
                look.y = 0f;
                if (look.sqrMagnitude > 0.001f)
                    self.rotation = Quaternion.Slerp(self.rotation, Quaternion.LookRotation(look), 8f * deltaTime);

                if (_timer >= _windUpTime)
                {
                    _dashDir = look.sqrMagnitude > 0.001f ? look.normalized : self.forward;
                    _state = State.Dashing;
                    _timer = 0f;
                }
            }
            else // Dashing
            {
                self.position += _dashDir * speed * _dashMultiplier * deltaTime;
                if (_timer >= _dashTime)
                {
                    _state = State.WindUp;
                    _timer = 0f;
                }
            }
        }
    }
}

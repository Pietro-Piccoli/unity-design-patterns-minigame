using UnityEngine;

namespace MinigameArena
{
    /// <summary>
    /// Gera inimigos em ondas. NAO cria inimigo na mao: ele pede pra IEnemyFactory.
    /// Aqui fica a decisao de QUANDO e QUAL tipo; o COMO fica na Factory.
    /// A dificuldade sobe com o tempo (spawn mais rapido e mais Tanks/Runners).
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        public float spawnInterval = 1.6f;
        public float minInterval = 0.45f;
        public float arenaHalfSize = 14f;

        private IEnemyFactory _factory;
        private Transform _target;
        private float _timer;
        private float _elapsed;

        public void Setup(IEnemyFactory factory, Transform target)
        {
            _factory = factory;
            _target = target;
        }

        private void Update()
        {
            if (GameManager.Instance == null || GameManager.Instance.IsGameOver) return;
            if (_factory == null || _target == null) return;

            _elapsed += Time.deltaTime;
            _timer -= Time.deltaTime;

            // intervalo diminui conforme o tempo passa (fica mais dificil)
            float currentInterval = Mathf.Max(minInterval, spawnInterval - _elapsed * 0.02f);

            if (_timer <= 0f)
            {
                _timer = currentInterval;
                _factory.Create(PickType(), RandomEdgePosition(), _target);
            }
        }

        /// <summary>Sorteia o tipo; com o tempo aparecem mais Runners e Tanks.</summary>
        private EnemyType PickType()
        {
            float t = _elapsed;
            float roll = Random.value;

            float tankChance = Mathf.Clamp(0.05f + t * 0.004f, 0.05f, 0.30f);
            float runnerChance = Mathf.Clamp(0.20f + t * 0.005f, 0.20f, 0.45f);

            if (roll < tankChance) return EnemyType.Tank;
            if (roll < tankChance + runnerChance) return EnemyType.Runner;
            return EnemyType.Grunt;
        }

        /// <summary>Escolhe um ponto aleatorio numa das bordas da arena.</summary>
        private Vector3 RandomEdgePosition()
        {
            float s = arenaHalfSize;
            float r = Random.Range(-s, s);
            int side = Random.Range(0, 4);
            return side switch
            {
                0 => new Vector3(r, 0f, s),
                1 => new Vector3(r, 0f, -s),
                2 => new Vector3(s, 0f, r),
                _ => new Vector3(-s, 0f, r),
            };
        }

        public void ResetSpawner()
        {
            _timer = 0f;
            _elapsed = 0f;
        }
    }
}

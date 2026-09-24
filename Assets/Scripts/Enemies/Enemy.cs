using UnityEngine;

namespace MinigameArena
{
    /// <summary>
    /// Inimigo. Nao decide COMO se move: ele delega isso pra uma IMovementStrategy
    /// (padrao STRATEGY). Quem monta o inimigo e escolhe a estrategia e a EnemyFactory
    /// (padrao FACTORY).
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Enemy : MonoBehaviour
    {
        private IMovementStrategy _strategy;
        private Transform _target;

        private float _speed;
        private int _health;
        private int _contactDamage;
        private int _scoreValue;

        public string StrategyName => _strategy != null ? _strategy.Name : "-";

        /// <summary>Configurado pela Factory logo depois de instanciar.</summary>
        public void Init(IMovementStrategy strategy, Transform target,
                         float speed, int health, int contactDamage, int scoreValue)
        {
            _strategy = strategy;
            _target = target;
            _speed = speed;
            _health = health;
            _contactDamage = contactDamage;
            _scoreValue = scoreValue;
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;
            _strategy?.Move(transform, _target, _speed, Time.deltaTime);
        }

        public void TakeDamage(int amount)
        {
            _health -= amount;
            if (_health <= 0) Die();
        }

        private void Die()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(_scoreValue);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            // encostou no player -> causa dano e some
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(_contactDamage);
                Destroy(gameObject);
            }
        }
    }
}

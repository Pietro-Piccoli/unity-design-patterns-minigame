using UnityEngine;
using UnityEngine.InputSystem;

namespace MinigameArena
{
    /// <summary>
    /// Controla o estado do jogo: pontos, game over e restart.
    /// Singleton simples pra qualquer script achar facil (Enemy, Player, UI...).
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public int Score { get; private set; }
        public float TimeSurvived { get; private set; }
        public bool IsGameOver { get; private set; }

        private Player _player;
        private EnemySpawner _spawner;

        public Player Player => _player;

        public void Setup(Player player, EnemySpawner spawner)
        {
            _player = player;
            _spawner = spawner;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Update()
        {
            if (IsGameOver)
            {
                // reinicia com R
                if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame) Restart();
                return;
            }

            TimeSurvived += Time.deltaTime;
        }

        public void AddScore(int amount)
        {
            if (IsGameOver) return;
            Score += amount;
        }

        public void GameOver()
        {
            IsGameOver = true;
        }

        public void Restart()
        {
            // limpa inimigos e tiros que sobraram
            foreach (Enemy e in Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None))
                Destroy(e.gameObject);
            foreach (Projectile p in Object.FindObjectsByType<Projectile>(FindObjectsSortMode.None))
                Destroy(p.gameObject);

            Score = 0;
            TimeSurvived = 0f;
            IsGameOver = false;

            _player?.ResetPlayer();
            _spawner?.ResetSpawner();
        }
    }
}

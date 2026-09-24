using UnityEngine;
using UnityEngine.InputSystem;

namespace MinigameArena
{
    /// <summary>
    /// Player: anda com WASD/setas e atira com o botao esquerdo do mouse
    /// na direcao do cursor. Tem vida; quando zera, GameManager da game over.
    /// Usa o Input System novo (Keyboard.current / Mouse.current).
    /// </summary>
    public class Player : MonoBehaviour
    {
        public float moveSpeed = 7f;
        public int maxHealth = 100;
        public float arenaHalfSize = 14f;   // limite da arena (nao deixa sair)
        public float fireCooldown = 0.18f;

        private int _health;
        private float _fireTimer;
        private Camera _cam;

        public int Health => _health;
        public int MaxHealth => maxHealth;

        private void Start()
        {
            _health = maxHealth;
            _cam = Camera.main;
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

            HandleMovement();
            HandleShooting();
        }

        private void HandleMovement()
        {
            Keyboard kb = Keyboard.current;
            if (kb == null) return;

            float h = 0f, v = 0f;
            if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) h -= 1f;
            if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) h += 1f;
            if (kb.sKey.isPressed || kb.downArrowKey.isPressed) v -= 1f;
            if (kb.wKey.isPressed || kb.upArrowKey.isPressed) v += 1f;

            Vector3 dir = new Vector3(h, 0f, v);
            if (dir.sqrMagnitude > 1f) dir.Normalize();

            transform.position += dir * moveSpeed * Time.deltaTime;

            // trava dentro da arena
            Vector3 p = transform.position;
            p.x = Mathf.Clamp(p.x, -arenaHalfSize, arenaHalfSize);
            p.z = Mathf.Clamp(p.z, -arenaHalfSize, arenaHalfSize);
            transform.position = p;
        }

        private void HandleShooting()
        {
            _fireTimer -= Time.deltaTime;

            Mouse mouse = Mouse.current;
            if (mouse == null || !mouse.leftButton.isPressed || _fireTimer > 0f) return;

            _fireTimer = fireCooldown;

            Vector3 aim = GetAimDirection();
            aim.y = 0f;
            if (aim.sqrMagnitude < 0.001f) aim = transform.forward;
            aim.Normalize();

            transform.rotation = Quaternion.LookRotation(aim);

            // altura fixa baixa pra acertar ate os inimigos pequenos
            Vector3 spawn = transform.position + aim * 1.1f;
            spawn.y = 0.5f;
            Projectile.Spawn(spawn, aim);
        }

        /// <summary>Descobre pra onde o mouse aponta no plano do chao.</summary>
        private Vector3 GetAimDirection()
        {
            if (_cam == null) _cam = Camera.main;
            if (_cam == null || Mouse.current == null) return transform.forward;

            Vector2 mp = Mouse.current.position.ReadValue();
            Ray ray = _cam.ScreenPointToRay(new Vector3(mp.x, mp.y, 0f));
            Plane ground = new Plane(Vector3.up, new Vector3(0f, transform.position.y, 0f));
            if (ground.Raycast(ray, out float dist))
            {
                Vector3 point = ray.GetPoint(dist);
                return point - transform.position;
            }
            return transform.forward;
        }

        public void TakeDamage(int amount)
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

            _health -= amount;
            if (_health <= 0)
            {
                _health = 0;
                GameManager.Instance?.GameOver();
            }
        }

        /// <summary>Chamado no restart.</summary>
        public void ResetPlayer()
        {
            _health = maxHealth;
            transform.position = new Vector3(0f, transform.position.y, 0f);
        }
    }
}

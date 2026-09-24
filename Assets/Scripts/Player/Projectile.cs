using UnityEngine;

namespace MinigameArena
{
    /// <summary>
    /// Tiro do player. Anda reto e mata inimigo ao encostar.
    /// A deteccao e feita por SphereCast ao longo do caminho do frame,
    /// pra NAO passar por cima de inimigo pequeno/rapido (evita tunneling).
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        public float speed = 22f;
        public float lifeTime = 2.5f;
        public int damage = 20;

        private Vector3 _dir;
        private float _radius = 0.5f; // raio generoso pra facilitar o acerto

        /// <summary>Cria um projetil ja configurado (primitivo, sem prefab).</summary>
        public static Projectile Spawn(Vector3 position, Vector3 direction)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "Projectile";
            go.transform.position = position;
            go.transform.localScale = Vector3.one * 0.35f;

            MaterialUtil.Paint(go, new Color(0.2f, 0.9f, 1f)); // ciano

            // colisor so como trigger (nao atrapalha ninguem); a deteccao real e por SphereCast
            Collider col = go.GetComponent<Collider>();
            if (col != null) col.isTrigger = true;

            Projectile p = go.AddComponent<Projectile>();
            p._dir = direction.normalized;
            return p;
        }

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            float step = speed * Time.deltaTime;

            // varre o caminho que a bala vai percorrer neste frame
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position, _radius, _dir, step, ~0, QueryTriggerInteraction.Collide);

            foreach (RaycastHit h in hits)
            {
                Enemy enemy = h.collider.GetComponentInParent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Destroy(gameObject);
                    return;
                }
            }

            transform.position += _dir * step;
        }
    }
}

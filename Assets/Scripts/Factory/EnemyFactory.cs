using UnityEngine;

namespace MinigameArena
{
    /// <summary>
    /// PADRAO FACTORY (implementacao).
    /// Centraliza TODA a criacao de inimigos. Pra cada tipo ela:
    ///  - cria o mesh (primitivo) e a cor,
    ///  - configura vida / velocidade / dano / pontos,
    ///  - adiciona Rigidbody + Collider (trigger),
    ///  - e principalmente ESCOLHE a estrategia de movimento (liga Factory + Strategy).
    /// Se amanha nascer um novo tipo de inimigo, muda so aqui.
    /// </summary>
    public class EnemyFactory : IEnemyFactory
    {
        public Enemy Create(EnemyType type, Vector3 position, Transform target)
        {
            GameObject go;
            Color color;
            float speed;
            int health, damage, score;
            IMovementStrategy strategy;

            switch (type)
            {
                case EnemyType.Runner:
                    go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    go.name = "Enemy_Runner";
                    go.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                    color = new Color(1f, 0.85f, 0.1f);      // amarelo
                    speed = 6.5f; health = 15; damage = 5; score = 15;
                    strategy = new ZigZagStrategy();          // rapido e imprevisivel
                    break;

                case EnemyType.Tank:
                    go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.name = "Enemy_Tank";
                    go.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
                    color = new Color(0.85f, 0.15f, 0.15f);   // vermelho
                    speed = 2.2f; health = 80; damage = 25; score = 40;
                    strategy = new ChargeStrategy();          // carrega e investe
                    break;

                case EnemyType.Grunt:
                default:
                    go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.name = "Enemy_Grunt";
                    go.transform.localScale = new Vector3(1f, 1f, 1f);
                    color = new Color(0.3f, 0.8f, 0.35f);     // verde
                    speed = 3.2f; health = 30; damage = 10; score = 10;
                    strategy = new ChaseStrategy();           // persegue reto
                    break;
            }

            // posiciona apoiado no chao
            Vector3 pos = position;
            pos.y = go.transform.localScale.y * 0.5f;
            go.transform.position = pos;

            // cor (funciona em Built-in e URP)
            MaterialUtil.Paint(go, color);

            // fisica: colisor como trigger + rigidbody cinematico (pra disparar OnTriggerEnter)
            Collider col = go.GetComponent<Collider>();
            col.isTrigger = true;
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;

            // logica + estrategia
            Enemy enemy = go.AddComponent<Enemy>();
            enemy.Init(strategy, target, speed, health, damage, score);

            return enemy;
        }
    }
}

using UnityEngine;

namespace MinigameArena
{
    /// <summary>
    /// PADRAO FACTORY (interface).
    /// Quem quer um inimigo so pede o TIPO e a posicao; nao precisa saber
    /// como o objeto e montado (mesh, cor, vida, velocidade, colisor, estrategia...).
    /// </summary>
    public interface IEnemyFactory
    {
        Enemy Create(EnemyType type, Vector3 position, Transform target);
    }
}

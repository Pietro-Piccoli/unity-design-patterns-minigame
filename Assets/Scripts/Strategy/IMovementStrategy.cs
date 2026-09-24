using UnityEngine;

namespace MinigameArena
{
    /// <summary>
    /// PADRAO STRATEGY (interface).
    /// Define um contrato de "como o inimigo se move".
    /// Cada inimigo guarda uma referencia a UMA estrategia e delega o movimento pra ela.
    /// Assim da pra ter varios comportamentos diferentes SEM if/switch gigante dentro do Enemy,
    /// e da pra TROCAR o comportamento em tempo de execucao.
    /// </summary>
    public interface IMovementStrategy
    {
        /// <summary>Nome pra mostrar na UI / debug.</summary>
        string Name { get; }

        /// <summary>Move o inimigo (self) em direcao ao alvo (target) neste frame.</summary>
        void Move(Transform self, Transform target, float speed, float deltaTime);
    }
}

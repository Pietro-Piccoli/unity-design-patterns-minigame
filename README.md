# Minigame Arena — Design Patterns (Factory + Strategy)

Minigame 3D feito em Unity para a atividade de Design Patterns.
Você controla um personagem numa arena, inimigos aparecem em ondas e vêm te pegar.
Você anda (WASD) e atira (mouse) pra sobreviver o máximo de tempo e fazer pontos.

## Padrões usados

Foram implementados **2 padrões**, os dois resolvendo necessidades reais do jogo:

### 1) Strategy (Estratégia)
**Problema real:** cada tipo de inimigo precisa se mover de um jeito diferente,
e eu não queria um `if/switch` gigante dentro do inimigo decidindo o movimento.

**Solução:** o inimigo (`Enemy`) não sabe COMO andar — ele guarda uma
`IMovementStrategy` e delega o movimento pra ela. Dá pra ter vários
comportamentos e até trocar em tempo de execução.

- `Strategy/IMovementStrategy.cs` — a interface
- `Strategy/ChaseStrategy.cs` — persegue o player em linha reta (inimigo Grunt)
- `Strategy/ZigZagStrategy.cs` — vai ziguezagueando, difícil de acertar (Runner)
- `Strategy/ChargeStrategy.cs` — carrega e dá uma investida (Tank)

### 2) Factory (Fábrica)
**Problema real:** criar inimigo é chato — tem mesh, cor, tamanho, vida,
velocidade, colisor, rigidbody E a estratégia certa. Não quero isso espalhado
pelo código nem repetido no spawner.

**Solução:** uma fábrica concentra toda a criação. O spawner só pede
"me dá um Tank nessa posição" e a fábrica monta tudo — inclusive já liga
a **Strategy** certa em cada tipo. É aqui que os dois padrões se conectam.

- `Factory/IEnemyFactory.cs` — a interface
- `Factory/EnemyFactory.cs` — cria e configura Grunt / Runner / Tank

## Como os inimigos ficam

| Tipo   | Cor       | Comportamento (Strategy) | Vida | Velocidade |
|--------|-----------|--------------------------|------|------------|
| Grunt  | verde     | Perseguir (ChaseStrategy) | média | média |
| Runner | amarelo   | Ziguezague (ZigZagStrategy) | baixa | alta |
| Tank   | vermelho  | Investida (ChargeStrategy) | alta | baixa (mas dá dash) |

## Como rodar

1. Abrir o projeto no Unity.
2. Apertar **Play**. Pronto — o jogo se monta sozinho por código
   (chão, luz, câmera, player, spawner e UI), não precisa arrastar nada.
3. Controles: **WASD/setas** pra andar, **mouse + clique esquerdo** pra atirar.
4. Sobreviva! Quando a vida zera, aperte **R** pra reiniciar.

> Obs: o jogo é montado em runtime pela classe `Core/GameBootstrap.cs`
> (`[RuntimeInitializeOnLoadMethod]`), então funciona mesmo numa cena vazia.

## Estrutura dos scripts

```
Assets/Scripts/
├── Strategy/        <- PADRAO STRATEGY
│   ├── IMovementStrategy.cs
│   ├── ChaseStrategy.cs
│   ├── ZigZagStrategy.cs
│   └── ChargeStrategy.cs
├── Factory/         <- PADRAO FACTORY
│   ├── IEnemyFactory.cs
│   └── EnemyFactory.cs
├── Enemies/
│   ├── EnemyType.cs
│   └── Enemy.cs
├── Player/
│   ├── Player.cs
│   └── Projectile.cs
├── Core/
│   ├── GameManager.cs
│   ├── EnemySpawner.cs
│   ├── UIManager.cs
│   ├── CameraFollow.cs
│   └── GameBootstrap.cs
└── Util/
    └── MaterialUtil.cs
```

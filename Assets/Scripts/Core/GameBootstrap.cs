using UnityEngine;

namespace MinigameArena
{
    /// <summary>
    /// Monta o jogo INTEIRO por codigo (chao, luz, camera, player, spawner, UI).
    /// Roda sozinho quando voce aperta Play, em qualquer cena (ate vazia).
    /// Assim nao precisa arrastar nada no editor: e so dar Play.
    /// </summary>
    public static class GameBootstrap
    {
        private const float ArenaHalf = 14f;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Build()
        {
            // se ja existe (ex: cena montada na mao), nao duplica
            if (Object.FindFirstObjectByType<GameManager>() != null) return;

            Time.timeScale = 1f;

            CreateGround();
            EnsureLight();
            Player player = CreatePlayer();
            SetupCamera(player.transform);

            // sistemas
            GameManager gm = new GameObject("GameManager").AddComponent<GameManager>();
            new GameObject("UIManager").AddComponent<UIManager>();

            EnemySpawner spawner = new GameObject("EnemySpawner").AddComponent<EnemySpawner>();
            spawner.arenaHalfSize = ArenaHalf;

            IEnemyFactory factory = new EnemyFactory();   // <- FACTORY em uso
            spawner.Setup(factory, player.transform);
            gm.Setup(player, spawner);
        }

        private static void CreateGround()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(3f, 1f, 3f); // 30x30 unidades
            MaterialUtil.Paint(ground, new Color(0.16f, 0.17f, 0.22f));
        }

        /// <summary>Garante que exista uma luz direcional (reaproveita a da cena se tiver).</summary>
        private static void EnsureLight()
        {
            foreach (Light l in Object.FindObjectsByType<Light>(FindObjectsSortMode.None))
                if (l.type == LightType.Directional) return;

            GameObject lightGo = new GameObject("Directional Light");
            Light light = lightGo.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.1f;
            light.color = new Color(1f, 0.97f, 0.9f);
            lightGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        private static Player CreatePlayer()
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            go.name = "Player";
            go.transform.position = new Vector3(0f, 1f, 0f);
            MaterialUtil.Paint(go, new Color(0.25f, 0.55f, 1f)); // azul

            Player p = go.AddComponent<Player>();
            p.arenaHalfSize = ArenaHalf;
            return p;
        }

        /// <summary>Reaproveita a Main Camera da cena; se nao tiver, cria uma.</summary>
        private static void SetupCamera(Transform target)
        {
            Camera cam = Camera.main;
            GameObject camGo;

            if (cam != null)
            {
                camGo = cam.gameObject;
            }
            else
            {
                camGo = new GameObject("Main Camera");
                camGo.tag = "MainCamera";
                camGo.AddComponent<Camera>();
            }

            // garante um unico AudioListener
            if (camGo.GetComponent<AudioListener>() == null)
                camGo.AddComponent<AudioListener>();

            CameraFollow follow = camGo.GetComponent<CameraFollow>();
            if (follow == null) follow = camGo.AddComponent<CameraFollow>();
            follow.target = target;

            camGo.transform.position = target.position + follow.offset;
            camGo.transform.LookAt(target.position);
        }
    }
}

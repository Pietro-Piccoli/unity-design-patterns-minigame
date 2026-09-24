using UnityEngine;

namespace MinigameArena
{
    /// <summary>
    /// HUD simples desenhado via OnGUI (nao precisa montar Canvas na mao).
    /// Mostra pontos, tempo, vida e a tela de game over.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        private GUIStyle _big;
        private GUIStyle _mid;
        private GUIStyle _center;
        private GUIStyle _centerMid;
        private Texture2D _barBg;
        private Texture2D _barFill;
        private Texture2D _overlay;

        private void EnsureStyles()
        {
            if (_big != null) return;

            _big = new GUIStyle { fontSize = 26, fontStyle = FontStyle.Bold };
            _big.normal.textColor = Color.white;

            _mid = new GUIStyle { fontSize = 18, fontStyle = FontStyle.Bold };
            _mid.normal.textColor = Color.white;

            _center = new GUIStyle { fontSize = 44, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
            _center.normal.textColor = Color.white;

            _centerMid = new GUIStyle { fontSize = 22, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
            _centerMid.normal.textColor = Color.white;

            _barBg = SolidTex(new Color(0f, 0f, 0f, 0.5f));
            _barFill = SolidTex(new Color(0.3f, 0.85f, 0.4f));
            _overlay = SolidTex(new Color(0f, 0f, 0f, 0.65f));
        }

        private static Texture2D SolidTex(Color c)
        {
            Texture2D t = new Texture2D(1, 1);
            t.SetPixel(0, 0, c);
            t.Apply();
            return t;
        }

        private void OnGUI()
        {
            EnsureStyles();
            GameManager gm = GameManager.Instance;
            if (gm == null) return;

            // pontos e tempo
            GUI.Label(new Rect(20, 15, 400, 40), $"Pontos: {gm.Score}", _big);
            GUI.Label(new Rect(20, 52, 400, 30), $"Tempo: {gm.TimeSurvived:0.0}s", _mid);

            // barra de vida
            if (gm.Player != null)
            {
                float pct = Mathf.Clamp01((float)gm.Player.Health / gm.Player.MaxHealth);
                float w = 240f;
                Rect bg = new Rect(20, 88, w, 22);
                GUI.DrawTexture(bg, _barBg);
                GUI.DrawTexture(new Rect(bg.x, bg.y, w * pct, bg.height), _barFill);
                GUI.Label(new Rect(bg.x + 6, bg.y, w, 22), $"Vida: {gm.Player.Health}", _mid);
            }

            // dica de controles
            GUI.Label(new Rect(20, Screen.height - 34, 900, 30),
                "WASD/setas: mover   |   Mouse + clique esquerdo: atirar", _mid);

            // game over
            if (gm.IsGameOver)
            {
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _overlay);
                GUI.Label(new Rect(0, Screen.height / 2 - 90, Screen.width, 60), "GAME OVER", _center);
                GUI.Label(new Rect(0, Screen.height / 2 - 10, Screen.width, 40),
                    $"Pontos: {gm.Score}   |   Tempo: {gm.TimeSurvived:0.0}s", _centerMid);
                GUI.Label(new Rect(0, Screen.height / 2 + 40, Screen.width, 40),
                    "Pressione R para reiniciar", _centerMid);
            }
        }
    }
}

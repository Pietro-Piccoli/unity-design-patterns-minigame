using UnityEngine;
using UnityEngine.Rendering;

namespace MinigameArena
{
    /// <summary>
    /// Helper pra pintar objetos funcionando tanto no render Built-in
    /// (shader "Standard", propriedade _Color) quanto no URP
    /// (shader "Universal Render Pipeline/Lit", propriedade _BaseColor).
    /// Detecta o pipeline ativo pra nao pintar de rosa por usar o shader errado.
    /// </summary>
    public static class MaterialUtil
    {
        private static Shader GetShader()
        {
            // se tem um Render Pipeline setado (URP/HDRP), usa o Lit do URP
            if (GraphicsSettings.currentRenderPipeline != null)
            {
                Shader urp = Shader.Find("Universal Render Pipeline/Lit");
                if (urp != null) return urp;
            }

            Shader std = Shader.Find("Standard");
            if (std != null) return std;

            Shader urpFallback = Shader.Find("Universal Render Pipeline/Lit");
            if (urpFallback != null) return urpFallback;

            return Shader.Find("Sprites/Default"); // ultimo recurso
        }

        public static void Paint(GameObject go, Color color)
        {
            Renderer r = go.GetComponent<Renderer>();
            if (r == null) return;

            Material m = new Material(GetShader());
            m.color = color;
            if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", color);
            r.material = m;
        }
    }
}

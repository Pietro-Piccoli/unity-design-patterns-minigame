using UnityEngine;

namespace MinigameArena
{
    /// <summary>Camera em vista aerea inclinada que segue o player suavemente.</summary>
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 18f, -12f);
        public float smooth = 6f;

        private void LateUpdate()
        {
            if (target == null) return;
            Vector3 desired = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desired, smooth * Time.deltaTime);
            transform.LookAt(target.position);
        }
    }
}

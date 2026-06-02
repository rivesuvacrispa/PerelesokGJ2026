using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance { get; private set; }

        private PlayerCamera() => Instance = this;
    }
}
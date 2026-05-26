using UnityEngine;

namespace Player
{
    public class PlayerInstance : MonoBehaviour
    {
        public static PlayerInstance Instance { get; private set; }

        private PlayerInstance() => Instance = this;
    }
}
using UnityEngine;

namespace Player
{
    public class PlayerInstance : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        
        public static PlayerInstance Instance { get; private set; }

        private PlayerInstance() => Instance = this;
        public Rigidbody Rigidbody => rb;
    }
}
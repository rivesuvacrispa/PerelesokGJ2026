using UnityEngine;
using Random = UnityEngine.Random;

namespace Effects
{
    [RequireComponent(typeof(Rigidbody))]
    public class SpellBall : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;

        public delegate void SpellBallCollisionEvent(SpellBall ball, Collision collision);
        public static event SpellBallCollisionEvent OnMirrorCollisionGlobal;
        private static int wallsLayer;
        
        
        private void Awake()
        {
            wallsLayer = LayerMask.NameToLayer("Walls");
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            rb.AddForce(Random.insideUnitSphere.normalized * 100);
        }

        private void OnCollisionEnter(Collision collision)
        {
            rb.linearVelocity *= 1.25f;
            
            if (collision.gameObject.layer == wallsLayer)
                OnMirrorCollisionGlobal?.Invoke(this, collision);
        }
    }
}
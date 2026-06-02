using System.Collections;
using UnityEngine;

namespace Effects
{
    [RequireComponent(typeof(Rigidbody))]
    public class SpellBall : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ParticleSystem particles;
        
        [Header("Settings")]
        [SerializeField] private float minVelocityToDespawn = 4f;
        [SerializeField] private int collisionsToDespawn = 8;
        
        private Rigidbody rb;
        private bool isDespawning = false;
        private bool isSpawning = true;
        private int collisionCounter = 0;
        public delegate void SpellBallCollisionEvent(SpellBall ball, Collision collision);
        public static event SpellBallCollisionEvent OnMirrorCollisionGlobal;
        private static int wallsLayer;
        public Rigidbody Rigidbody => rb;


        
        private void Awake()
        {
            wallsLayer = LayerMask.NameToLayer("Walls");
            rb = GetComponent<Rigidbody>();
            StartCoroutine(SpawnRoutine());
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (isDespawning || isSpawning) return;
            
            rb.linearVelocity *= 1.25f;
            collisionCounter++;

            if (collision.gameObject.layer == wallsLayer)
                OnMirrorCollisionGlobal?.Invoke(this, collision);
            
            if (collisionCounter >= collisionsToDespawn)
                Despawn();
        }

        private void Update()
        {
            if (rb.linearVelocity.magnitude < minVelocityToDespawn)
                Despawn();
        }

        private void Despawn()
        {
            if (isDespawning || isSpawning) return;
            StartCoroutine(DespawnRoutine());
        }

        private IEnumerator DespawnRoutine()
        {
            isDespawning = true;
            particles.Stop();
            yield return new WaitUntil(() => !particles.isPlaying);
            Destroy(gameObject);
        }

        private IEnumerator SpawnRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            isSpawning = false;
        }
    }
}
using System.Collections;
using Global;
using UnityEngine;

namespace Effects.SpellBalls
{
    [RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Collider))]
    public class SpellBall : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected ParticleSystem particles;
        [SerializeField] private ParticleSystem explosionParticles;
        
        [Header("Settings")]
        [SerializeField] private float minVelocityToDespawn = 4f;
        [SerializeField] protected int collisionsToDespawn = 8;
        
        private Rigidbody rb;
        private Collider col;
        protected bool isDespawning = false;
        protected bool isSpawning = true;
        protected int collisionCounter = 0;
        public delegate void SpellBallCollisionEvent(SpellBall ball, Collision collision);
        public static event SpellBallCollisionEvent OnMirrorCollisionGlobal;
        public Rigidbody Rigidbody => rb;


        
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
            StartCoroutine(SpawnRoutine());
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (isDespawning) return;
            
            rb.linearVelocity *= 1.25f;
            collisionCounter++; 
            
            OnAnyCollision();

            if (collision.gameObject.layer == GlobalDefinitions.MirrorsLayer)
            {
                CollideWithMirror(collision.contacts[0].point);
                OnMirrorCollisionGlobal?.Invoke(this, collision);
            }
            
            else if (collision.gameObject.layer == GlobalDefinitions.SpellBallLayer)
                CollideWithSpellBall(collision.contacts[0].point, collision.gameObject.GetComponent<SpellBall>());
            
            if (collisionCounter >= collisionsToDespawn)
            {
                Despawn();
            }
        }

        protected virtual void CollideWithSpellBall(Vector3 point, SpellBall other)
        {
            
        }

        protected virtual void CollideWithMirror(Vector3 contactPoint)
        {
            
        }

        protected virtual void OnAnyCollision()
        {
            
        }

        private void Update()
        {
            if (rb.linearVelocity.magnitude < minVelocityToDespawn && !isSpawning)
                Despawn();
        }

        private void Despawn()
        {
            StartCoroutine(DespawnRoutine());
        }

        protected virtual void Explode()
        {
            particles.Stop();
            explosionParticles.Play();
            StartCoroutine(DespawnRoutine());
        }
        

        private IEnumerator DespawnRoutine()
        {
            if (isDespawning) yield return null;

            isDespawning = true;
            particles.Stop();
            col.enabled = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            yield return new WaitUntil(() => !(particles.IsAlive() || explosionParticles.IsAlive()));
            
            Destroy(gameObject);
        }
        
        private IEnumerator SpawnRoutine()
        {
            yield return new WaitForSeconds(0.2f);
            isSpawning = false;
        }
    }
}
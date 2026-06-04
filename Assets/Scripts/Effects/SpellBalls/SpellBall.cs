using System.Collections;
using Global;
using UnityEngine;

namespace Effects.SpellBalls
{
    [RequireComponent(typeof(Rigidbody))]
    public class SpellBall : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private ParticleSystem explosionParticles;
        
        [Header("Settings")]
        [SerializeField] private float minVelocityToDespawn = 4f;
        [SerializeField] private int collisionsToDespawn = 8;
        
        private Rigidbody rb;
        private bool isDespawning = false;
        private int collisionCounter = 0;
        public delegate void SpellBallCollisionEvent(SpellBall ball, Collision collision);
        public static event SpellBallCollisionEvent OnMirrorCollisionGlobal;
        public Rigidbody Rigidbody => rb;


        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (isDespawning) return;
            
            rb.linearVelocity *= 1.25f;
            collisionCounter++;

            if (collision.gameObject.layer == GlobalDefinitions.MirrorsLayer)
                OnMirrorCollisionGlobal?.Invoke(this, collision);
            
            else if (collision.gameObject.layer == GlobalDefinitions.SpellBallLayer)
                CollideWithSpellBall(collision.contacts[0].point, collision.gameObject.GetComponent<SpellBall>());
            
            if (collisionCounter >= collisionsToDespawn)
                Despawn();
        }

        protected virtual void CollideWithSpellBall(Vector3 point, SpellBall other)
        {
            
        }

        private void Update()
        {
            if (rb.linearVelocity.magnitude < minVelocityToDespawn)
                Despawn();
        }

        protected void Despawn()
        {
            StartCoroutine(DespawnRoutine());
        }

        public void Explode()
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
            rb.constraints = RigidbodyConstraints.FreezeAll;
            yield return new WaitUntil(() => !particles.IsAlive());
            Destroy(gameObject);
        }
    }
}
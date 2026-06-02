using UnityEngine;

namespace Effects
{
    public class BarrierParticles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particles;
        
        private void OnEnable()
        {
            SpellBall.OnMirrorCollisionGlobal += OnSpellBallCollideWithMirror;
        }

        private void OnDisable()
        {
            SpellBall.OnMirrorCollisionGlobal -= OnSpellBallCollideWithMirror;
        }

        private void OnSpellBallCollideWithMirror(SpellBall ball, Collision collision)
        {
            var contacts = collision.contacts;
            if (contacts.Length == 0) return;
            
            foreach (ContactPoint point in contacts)
            {
                particles.transform.position = point.point;

                particles.transform.rotation =
                    Quaternion.LookRotation(point.normal);

                particles.Play();
            }
        }
    }
}
using System.Collections;
using UnityEngine;
using Util;

namespace Effects.EffectZones
{
    public class HeatedMirrorEffectZone : SpellEffectZone
    {
        [SerializeField] private ParticleSystem glowGlassParticles;
        
        public const float SIZE = 3f;

        
        private void Start()
        {
            GetComponent<SphereCollider>().radius = SIZE;

            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            if (PhysUtils.TryFindAllMirrorsInRange(transform.position, SIZE, out var points))
            {
                foreach (Vector3 p in points)
                {
                    glowGlassParticles.transform.position = p;
                    glowGlassParticles.transform.LookAt(transform);
                    glowGlassParticles.Play();
                    
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
}
using System.Collections;
using UnityEngine;
using Util;

namespace Effects.EffectZones
{
    public class AshEffectZone : SpellEffectZone
    {
        [SerializeField] private ParticleSystem ashParticles;
        
        public const float SIZE = 2f;

        
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
                    ashParticles.transform.position = p;
                    ashParticles.transform.LookAt(transform);
                    ashParticles.Play();
                    
                    // Ждет конца текущего кадра, т.к. трансформ не обновит свою позицию до этого момента
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
}
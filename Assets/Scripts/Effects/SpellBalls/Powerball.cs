using System.Collections;
using Effects.EffectZones;
using Global;
using UnityEngine;
using Util;

namespace Effects.SpellBalls
{
    public class Powerball : SpellBall
    {
        [SerializeField] private ParticleSystem ballParticles;
        private ParticleSystem.MainModule main1;
        private ParticleSystem.MainModule main2;
        private float startSize1;
        private float startSize2;


        protected override void Awake()
        {
            base.Awake();
            main1 = particles.main;
            main2 = ballParticles.main;
            startSize1 = main1.startSize.constant;
            startSize2 = main2.startSize.constant;
        }

        protected override void OnAnyCollision()
        {
            main1.startSize = startSize1 * collisionCounter;
            main2.startSize = startSize2 * collisionCounter;
        }

        protected override void CollideWithMirror(Vector3 contactPoint)
        {
            if (collisionCounter >= collisionsToDespawn)
                Explode();
        }

        protected override void Explode()
        {
            base.Explode();
            StartCoroutine(ExplodeRoutine());
        }

        private IEnumerator ExplodeRoutine()
        {
            yield return new WaitForSeconds(3f);

            bool insideCrack =
                PhysUtils.InsideEffectZone<MirrorCrackEffectZone>(transform.position, out _, out var lastZone);
            
            if (insideCrack && lastZone is not null)
                lastZone.IncreaseCracks();
        }
    }
}
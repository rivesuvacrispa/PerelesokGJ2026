using Effects.EffectZones;
using Global;
using UnityEngine;
using Util;

namespace Effects.SpellBalls
{
    public class Fireball : SpellBall
    {

        protected override void CollideWithSpellBall(Vector3 contactPoint, SpellBall other)
        {
            if (other is Fireball)
            {
                bool nearMirrors = PhysUtils.TryFindAllMirrorsInRange(contactPoint, AshEffectZone.SIZE, out _);
                
                if (nearMirrors)
                {
                    Instantiate(GlobalDefinitions.AshEffectZone, contactPoint, Quaternion.identity);
                }
                
                Explode();
            }
        }

        protected override void CollideWithMirror(Vector3 contactPoint)
        {
            if (PhysUtils.InsideEffectZone<AshEffectZone>(contactPoint, out _))
            {
                Instantiate(GlobalDefinitions.HeatedMirrorEffectZone, transform.position, Quaternion.identity);
                Explode();
            }
        }
    }
}
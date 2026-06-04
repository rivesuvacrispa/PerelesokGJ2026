using Effects.EffectZones;
using Global;
using UnityEngine;
using Util;

namespace Effects.SpellBalls
{
    public class IceBall : SpellBall
    {
        protected override void CollideWithMirror(Vector3 contactPoint)
        {
            bool insideAsh = PhysUtils.InsideEffectZone<AshEffectZone>(contactPoint, out _, out _);
            bool insideHeat =
                PhysUtils.InsideEffectZone<HeatedMirrorEffectZone>(contactPoint, out int heatStacks, out _);
            
            if (insideAsh || insideHeat)
            {
                Explode();
            }
            
            if (insideHeat)
            {
                if (heatStacks >= 3)
                    Instantiate(GlobalDefinitions.MirrorCrackEffectZone, transform.position, Quaternion.identity);
            }
        }
    }
}
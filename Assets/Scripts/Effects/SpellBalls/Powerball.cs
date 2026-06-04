using System.Collections;
using Global;
using UnityEngine;

namespace Effects.SpellBalls
{
    public class Powerball : SpellBall
    {
        protected override void CollideWithMirror(Vector3 contactPoint)
        {
            Instantiate(GlobalDefinitions.MirrorCrackEffectZone, transform.position, Quaternion.identity);

            Explode();
            // StartCoroutine(ExplodeRoutine());
        }

        private IEnumerator ExplodeRoutine()
        {
            yield return new WaitForSeconds(3f);
            Instantiate(GlobalDefinitions.MirrorCrackEffectZone, transform.position, Quaternion.identity);
        }
    }
}
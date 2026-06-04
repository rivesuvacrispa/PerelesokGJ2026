using UnityEngine;
using Util;

namespace Effects.EffectZones
{
    [RequireComponent(typeof(Collider))]
    public abstract class SpellEffectZone : DestroyOnSleep
    {
    }
}
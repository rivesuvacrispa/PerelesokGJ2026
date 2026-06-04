using Interaction.Objects;
using UnityEngine;

namespace Effects.EffectZones
{
    [RequireComponent(typeof(Collider))]
    public abstract class SpellEffectZone : MonoBehaviour
    {
        private void OnEnable()
        {
            Bed.OnSleep += OnSleep;
        }

        private void OnDisable()
        {
            Bed.OnSleep -= OnSleep;
        }

        private void OnSleep()
        {
            Destroy(gameObject);
        }
    }
}
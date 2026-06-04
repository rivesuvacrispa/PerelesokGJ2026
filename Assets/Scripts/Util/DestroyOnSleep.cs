using Interaction.Objects;
using UnityEngine;

namespace Util
{
    public class DestroyOnSleep : MonoBehaviour
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
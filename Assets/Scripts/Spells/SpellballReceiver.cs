using Effects.SpellBalls;
using UnityEngine;

namespace Spells
{
    [RequireComponent(typeof(Collider))]
    public class SpellballReceiver : MonoBehaviour
    {
        public delegate void SpellBallReceiverEvent(SpellBall spellBall);
        public event SpellBallReceiverEvent OnSpellBallReceived; 

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out SpellBall spellBall))
                OnSpellBallReceived?.Invoke(spellBall);
        }
    }
}
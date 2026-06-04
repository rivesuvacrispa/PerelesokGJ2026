using System.Collections;
using UnityEngine;

namespace Interaction.Objects
{
    public class Safe : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particles;

        public void Play()
        {
            particles.Play();
        }
        
        public IEnumerator PlayAnimation()
        {
            particles.Play();
            yield return new WaitUntil(() => !particles.IsAlive());
        }
    }
}
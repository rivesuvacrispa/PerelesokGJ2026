using UnityEngine;

namespace Story.Events
{
    public class StoryEvent : MonoBehaviour
    {
        [SerializeField] private PlotMemory memory;

        protected virtual void OnFire()
        {
            
        }
        
        public void Fire()
        {
            OnFire();
            PlotManager.Add(memory);
        }
    }
}
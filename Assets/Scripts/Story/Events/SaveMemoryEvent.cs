using UnityEngine;

namespace Story.Events
{
    public class SaveMemoryEvent : MonoBehaviour
    {
        [SerializeField] private PlotMemory memory;

        protected virtual void OnFire()
        {
            
        }
        
        public void Fire()
        {
            OnFire();
            PlotManager.AddMemory(memory);
        }
    }
}
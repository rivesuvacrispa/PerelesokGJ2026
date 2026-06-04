using UnityEngine;

namespace Story.Events
{
    public class SaveMemoryEvent : DialogEvent
    {
        [SerializeField] private PlotMemory memory;

        protected virtual void OnFire()
        {
            
        }
        
        public override void Fire()
        {
            OnFire();
            MemoryManager.AddMemory(memory);
        }
    }
}
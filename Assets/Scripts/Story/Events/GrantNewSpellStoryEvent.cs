using Spells;
using UnityEngine;

namespace Story.Events
{
    public class GrantNewSpellStoryEvent : SaveMemoryEvent
    {
        [SerializeField] private ScriptableSpell spellToGrant;
        
        protected override void OnFire()
        {
            SpellCaster.Instance.SetSpellEnabled(spellToGrant, true);
        }
    }
}
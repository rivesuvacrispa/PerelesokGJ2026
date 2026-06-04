using System.Collections.Generic;
using System.Linq;
using Controls;
using UnityEngine;

namespace Spells
{
    public class SpellCaster : MonoBehaviour
    {
        public static SpellCaster Instance { get; private set; }
        
        [SerializeField] private ScriptableSpell[] spells;
        [SerializeField] private float spellBallCastSpeed = 50;
        [SerializeField] private List<GameObject> spellButtons = new();
        

        public static float SpellBallCastSpeed { get; private set; }
        private int spellsCount;


        private SpellCaster() => Instance = this;
        
        private void Awake()
        {
            spellsCount = spells.Length;
            SpellBallCastSpeed = spellBallCastSpeed;
        }

        private void OnEnable()
        {
            ControlsManager.OnNumberPress += CastSpell;
        }

        private void OnDisable()
        {
            ControlsManager.OnNumberPress -= CastSpell;
        }

        /**
         * Кастует спелл по индексу, начиная с 0 (фаербол)
         */
        private void CastSpell(int numberPressed)
        {
            int spellIndex = numberPressed - 1;
            
            if (spellIndex < 0 || spellIndex >= spellsCount) return;
            if (!spellButtons[spellIndex].gameObject.activeSelf) return;
            
            spells[spellIndex].Cast();
        }

        public void SetSpellEnabled(ScriptableSpell spell, bool state)
        {
            int index = spells.ToList().IndexOf(spell);
            spellButtons[index].SetActive(state);
        }
    }
}
using System;
using Controls;
using UnityEngine;

namespace Spells
{
    public class SpellCaster : MonoBehaviour
    {
        [SerializeField] private Spell[] spellList;
        [SerializeField] private float spellBallCastSpeed = 75;

        public static float SpellBallCastSpeed { get; private set; }


        
        private void Awake()
        {
            SpellBallCastSpeed = spellBallCastSpeed;
        }

        private void OnEnable()
        {
            ControlsManager.OnSpellPress += CastSpell;
        }

        private void OnDisable()
        {
            ControlsManager.OnSpellPress -= CastSpell;
        }

        private void CastSpell()
        {
            if (spellList.Length == 0) return;
            spellList[0].Cast();
        }
    }
}
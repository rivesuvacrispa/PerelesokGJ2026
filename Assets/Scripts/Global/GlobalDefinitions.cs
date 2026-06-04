using Effects;
using Effects.EffectZones;
using Interaction.Objects;
using UnityEngine;

namespace Global
{
    public class GlobalDefinitions : MonoBehaviour
    {
        [Header("Global objects")]
        [SerializeField] private SignLetter signLetter;

        [Header("Effect Zone Prefabs")]
        [SerializeField] private AshEffectZone ashEffectZone;
        [SerializeField] private HeatedMirrorEffectZone heatedMirrorEffectZone;
        [SerializeField] private MirrorCrackEffectZone mirrorCrackEffectZone;

        [Header("Other definitions")]
        [SerializeField] private Sprite[] crackTypeToCrackSprite;
        [SerializeField] private Material mirrorCrackMaterial;
        
        
        private static GlobalDefinitions instance;

        private GlobalDefinitions() => instance = this;


        public static SignLetter SignLetter => instance.signLetter;
        public static AshEffectZone AshEffectZone => instance.ashEffectZone;
        public static HeatedMirrorEffectZone HeatedMirrorEffectZone => instance.heatedMirrorEffectZone;
        public static MirrorCrackEffectZone MirrorCrackEffectZone => instance.mirrorCrackEffectZone;
        public static int MirrorsLayer { get; private set; }
        public static int SpellBallLayer { get; private set; }
        public static int SpellEffectZoneLayer { get; private set; }
        public static Sprite GetCrackSprite(MirrorCrackType crackType) => instance.crackTypeToCrackSprite[(int)crackType];
        public static Material MirrorCrackMaterial => instance.mirrorCrackMaterial;


        private void Awake()
        {
            MirrorsLayer = LayerMask.NameToLayer("Mirrors");
            SpellBallLayer = LayerMask.NameToLayer("Spellball");
            SpellEffectZoneLayer = LayerMask.NameToLayer("SpellEffectZone");
        }
    }
}
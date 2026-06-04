using System.Collections.Generic;
using Global;
using Interaction.Objects;
using Story;
using UnityEngine;
using Util;

namespace Effects.EffectZones
{
    public class MirrorCrackEffectZone : SpellEffectZone
    {
        [SerializeField] private PlotMemory playerCrackedMirrorMem;
        [SerializeField] private PlotMemory playerCrackedMirrorSecondTimeMem;
        
        public const float SIZE = 2f;
        public MirrorCrackType CrackType { get; private set; } = MirrorCrackType.Medium;
        private List<SpriteRenderer> instantiatedCracks = new();


        private void Start()
        {
            GetComponent<SphereCollider>().radius = SIZE;

            SaveFirstCrackMemory();
            
            PhysUtils.InsideEffectZone(transform.position, out var zonesCount, out MirrorCrackEffectZone existing);
            if (zonesCount > 1)
            {
                Destroy(gameObject);
                return;
            }
            
            CreateCracks();
            CreateMirrorShard();
        }

        private void CreateCracks()
        {
            if (PhysUtils.TryFindAllMirrorsInRange(transform.position, SIZE, out var points))
            {
                foreach (Vector3 p in points)
                {
                    GameObject crack = new GameObject();
                    Vector3 lookDirection = (transform.position - p).normalized;
                    crack.AddComponent<DestroyOnSleep>();
                    crack.transform.position = p + lookDirection * 0.01f;
                    crack.name = "Mirror crack";
                    crack.transform.rotation = Quaternion.LookRotation(lookDirection);
                    SpriteRenderer crackRenderer = crack.AddComponent<SpriteRenderer>();
                    crackRenderer.sortingOrder = 2;
                    crackRenderer.SetMaterials(new List<Material>()
                    {
                        GlobalDefinitions.MirrorCrackMaterial
                    });
                    crackRenderer.sprite = GlobalDefinitions.GetCrackSprite(CrackType);
                    instantiatedCracks.Add(crackRenderer);
                }
            }
        }

        public void IncreaseCracks()
        {
            MirrorCrackType newCrackType = (MirrorCrackType) Mathf.Clamp((int) CrackType + 1, 0, 2);
            CrackType = newCrackType;
            Sprite newSprite = GlobalDefinitions.GetCrackSprite(newCrackType);
            foreach (SpriteRenderer crack in instantiatedCracks)
            {
                crack.sprite = newSprite;
            }

            SaveSecondCrackMemory();
            CreateMirrorShard();
        }

        private void SaveFirstCrackMemory()
        {
            if (!MemoryManager.HasMemory(playerCrackedMirrorMem))
                MemoryManager.AddMemory(playerCrackedMirrorMem);
        }
        
        private void SaveSecondCrackMemory()
        {
            if (!MemoryManager.HasMemory(playerCrackedMirrorSecondTimeMem))
                MemoryManager.AddMemory(playerCrackedMirrorSecondTimeMem);
        }

        private void CreateMirrorShard()
        {
            Instantiate(GlobalDefinitions.MirrorShardPrefab, transform.position, Random.rotation);
        }
    }
}
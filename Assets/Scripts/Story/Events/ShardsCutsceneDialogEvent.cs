using System.Collections;
using Global;
using Interaction.Objects;
using Player;
using UnityEngine;
using Util;

namespace Story.Events
{
    public class ShardsCutsceneDialogEvent : DialogEvent
    {
        [SerializeField] private PlotMemory badEndingMemory;

        
        public override void Fire()
        {
            ShardsCutsceneDialogEvent copy = (new GameObject()).AddComponent<ShardsCutsceneDialogEvent>();
            MemoryManager.AddMemory(badEndingMemory);
            copy.StartCoroutine(copy.FireRoutine());
        }

        private IEnumerator FireRoutine()
        {
            PlayerCamera.Instance.RotateToLook(PhysUtils.roomCenter);
            yield return GatherShardsRoutine();
            yield return new WaitForSeconds(1f);
            
            // Vector3 cameraLookAt = PhysUtils.FindClosestMirrorPoint(PlayerCamera.Instance.transform.position);
            // PlayerCamera.Instance.RotateToLook(cameraLookAt);
        }

        private IEnumerator GatherShardsRoutine()
        {
            foreach (MirrorShard shard in MirrorShard.SHARDS) shard.PlayParticles();
            GlobalDefinitions.Sword.gameObject.SetActive(true);

            float t = 0f;
            while (t < 2f)
            {
                foreach (MirrorShard shard in MirrorShard.SHARDS)
                {
                    Vector3 currentPos = shard.Rigidbody.position;
                    float delta = Vector3.Distance(currentPos, PhysUtils.roomCenter);
                    shard.Rigidbody.MovePosition(Vector3.MoveTowards(currentPos, PhysUtils.roomCenter, delta * t / 2f));
                }
                t += Time.deltaTime;
                yield return null;
            }
            
            MirrorShard.ClearAll();
            yield return new WaitForSeconds(3f);
        }
    }
}
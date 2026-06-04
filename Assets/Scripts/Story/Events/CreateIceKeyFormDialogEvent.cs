using System.Collections;
using Global;
using Player;
using UnityEngine;
using Util;

namespace Story.Events
{
    public class CreateIceKeyFormDialogEvent : DialogEvent
    {
        [SerializeField] private PlotMemory goodEndingMemory;
        
        public override void Fire()
        {
            CreateIceKeyFormDialogEvent copy = (new GameObject()).AddComponent<CreateIceKeyFormDialogEvent>();
            MemoryManager.AddMemory(goodEndingMemory);
            copy.StartCoroutine(copy.FireRoutine());
        }

        private IEnumerator FireRoutine()
        {
            PlayerCamera.Instance.RotateToLook(PhysUtils.roomCenter);
            yield return new WaitForSeconds(1f);

            yield return GlobalDefinitions.IceKeyFrame.SpawnAnimationRoutine();
            yield return new WaitForSeconds(1f);

            Vector3 cameraLookAt = PhysUtils.FindClosestMirrorPoint(PlayerCamera.Instance.transform.position);
            PlayerCamera.Instance.RotateToLook(cameraLookAt);
        }
    }
}
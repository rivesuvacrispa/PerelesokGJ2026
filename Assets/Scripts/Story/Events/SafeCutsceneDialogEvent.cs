using System.Collections;
using Global;
using Player;
using UnityEngine;
using Util;

namespace Story.Events
{
    public class SafeCutsceneDialogEvent : DialogEvent
    {
        public override void Fire()
        {
            SafeCutsceneDialogEvent copy = (new GameObject()).AddComponent<SafeCutsceneDialogEvent>();
            copy.StartCoroutine(copy.FireRoutine());
        }

        private IEnumerator FireRoutine()
        {
            PlayerCamera.Instance.RotateToLook(GlobalDefinitions.Safe.transform.position);
            yield return new WaitForSeconds(1f);            
            
            yield return GlobalDefinitions.Safe.PlayAnimation();
            
            Vector3 cameraLookAt = PhysUtils.FindClosestMirrorPoint(PlayerCamera.Instance.transform.position);
            PlayerCamera.Instance.RotateToLook(cameraLookAt);
            yield return new WaitForSeconds(1f);
        }
    }
}
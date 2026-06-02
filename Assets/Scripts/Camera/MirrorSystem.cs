using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class MirrorSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int recursionDepth = 2;
        [SerializeField] private LayerMask reflectLayers = -1;

        private UnityEngine.Camera mainCam;
        private UnityEngine.Camera frustumCam;
        private List<MirrorPlane> mirrors = new List<MirrorPlane>();
        private int currentFrame = -1;
        private RenderTexture blackTexture;

        private List<RenderTexture> activeTextures = new List<RenderTexture>();

        private struct VirtualCamera
        {
            public Vector3 position;
            public Quaternion rotation;
        }

        private struct ViewTexture
        {
            public MirrorPlane mirror;
            public RenderTexture rt;
        }

        private void Awake()
        {
            mainCam = GetComponent<UnityEngine.Camera>();
            blackTexture = new RenderTexture(1, 1, 0);
            blackTexture.Create();

            GameObject camObj = new GameObject("MirrorFrustumCam");
            camObj.hideFlags = HideFlags.HideAndDontSave;
            frustumCam = camObj.AddComponent<UnityEngine.Camera>();
            frustumCam.enabled = false;
        }

        private void OnDestroy()
        {
            foreach (var rt in activeTextures)
            {
                RenderTexture.ReleaseTemporary(rt);
            }
            activeTextures.Clear();
            if (frustumCam != null) Destroy(frustumCam.gameObject);
        }

        private void LateUpdate()
        {
            if (Time.frameCount == currentFrame) return;
            currentFrame = Time.frameCount;

            mirrors.Clear();
            mirrors.AddRange(FindObjectsByType<MirrorPlane>(FindObjectsSortMode.None));

            if (mirrors.Count == 0) return;

            foreach (var rt in activeTextures)
            {
                RenderTexture.ReleaseTemporary(rt);
            }
            activeTextures.Clear();

            VirtualCamera mainVCam = new VirtualCamera
            {
                position = mainCam.transform.position,
                rotation = mainCam.transform.rotation
            };

            List<ViewTexture> finalTextures = RenderRecursive(mainVCam, recursionDepth);

            foreach (var m in mirrors)
            {
                m.SetDisplayTexture(blackTexture);
            }

            foreach (var vt in finalTextures)
            {
                vt.mirror.SetDisplayTexture(vt.rt);
                activeTextures.Add(vt.rt);
            }
        }

        private List<ViewTexture> RenderRecursive(VirtualCamera viewCam, int remainingDepth)
        {
            List<ViewTexture> results = new List<ViewTexture>();

            if (remainingDepth <= 0) return results;

            frustumCam.CopyFrom(mainCam);
            frustumCam.transform.position = viewCam.position;
            frustumCam.transform.rotation = viewCam.rotation;
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(frustumCam);

            List<MirrorPlane> visibleMirrors = new List<MirrorPlane>();
            foreach (var mirror in mirrors)
            {
                if (!mirror.rend.enabled) continue;

                Vector3 toCam = viewCam.position - mirror.transform.position;
                if (Vector3.Dot(toCam, mirror.transform.up) <= 0) continue;

                if (GeometryUtility.TestPlanesAABB(frustumPlanes, mirror.rend.bounds))
                {
                    visibleMirrors.Add(mirror);
                }
            }

            foreach (var mirror in visibleMirrors)
            {
                VirtualCamera vCam = ReflectCameraInPlane(viewCam, mirror.transform);


                List<ViewTexture> innerTextures = RenderRecursive(vCam, remainingDepth - 1);

                foreach (var m in mirrors) m.SetReflectionSource(blackTexture, remainingDepth);
                foreach (var tex in innerTextures) tex.mirror.SetReflectionSource(tex.rt, remainingDepth);

                RenderTexture rt = RenderTexture.GetTemporary(Screen.width, Screen.height, 24);

                mirror.rend.enabled = false;
                mirror.RenderReflection(rt, vCam.position, vCam.rotation, mainCam);
                mirror.rend.enabled = true;

                foreach (var tex in innerTextures) RenderTexture.ReleaseTemporary(tex.rt);

                results.Add(new ViewTexture { mirror = mirror, rt = rt });
            }

            return results;
        }

        private VirtualCamera ReflectCameraInPlane(VirtualCamera vc, Transform plane)
        {
            return ReflectCameraInPlane(vc.position, vc.rotation, plane);
        }

        private VirtualCamera ReflectCameraInPlane(Vector3 camPos, Quaternion camRot, Transform plane)
        {
            Vector3 planeNormal = plane.up;
            Vector3 planePos = plane.position;

            Vector3 toCam = camPos - planePos;
            float dot = Vector3.Dot(toCam, planeNormal);
            Vector3 reflPos = camPos - 2 * dot * planeNormal;

            Vector3 reflForward = camRot * Vector3.forward;
            reflForward = reflForward - 2 * Vector3.Dot(reflForward, planeNormal) * planeNormal;
            Vector3 reflUp = camRot * Vector3.up;
            reflUp = reflUp - 2 * Vector3.Dot(reflUp, planeNormal) * planeNormal;

            Quaternion reflRot = Quaternion.LookRotation(reflForward, reflUp);

            return new VirtualCamera { position = reflPos, rotation = reflRot };
        }

        public UnityEngine.Camera GetMainCamera()
        {
            return mainCam;
        }

        public LayerMask GetReflectLayers()
        {
            return reflectLayers;
        }
    }
}

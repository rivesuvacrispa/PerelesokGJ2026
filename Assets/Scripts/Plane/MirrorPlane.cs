using System;
using UnityEngine;

namespace Mirror
{
    public class MirrorPlane : MonoBehaviour
    {
        [HideInInspector] public Renderer rend;
        private Material mirrorMaterial;

        [HideInInspector] public UnityEngine.Camera reflectionCam;

        public float clipPlaneOffset = 0.01f;
        public LayerMask reflectLayers = -1;

        private void Awake()
        {
            rend = GetComponent<Renderer>();
            mirrorMaterial = rend.material;

            CreateReflectionCamera();
        }

        void CreateReflectionCamera()
        {
            GameObject camObj = new GameObject($"{name}_ReflectionCam");
            camObj.hideFlags = HideFlags.HideInHierarchy;
            reflectionCam = camObj.AddComponent<UnityEngine.Camera>();
            reflectionCam.enabled = false;
            reflectionCam.allowHDR = true;
        }

        public void SetReflectionSource(Texture source, int depth)
        {
            mirrorMaterial.SetFloat("_Depth", Mathf.Pow(depth * 0.3f, 2f));
            mirrorMaterial.SetTexture("_BaseMap", source);
        }

        public void SetDisplayTexture(RenderTexture texture)
        {
            mirrorMaterial.mainTexture = texture;
        }

        public void RenderReflection(RenderTexture targetTex, Vector3 camPos, Quaternion camRot, UnityEngine.Camera mainCam)
        {
            reflectionCam.CopyFrom(mainCam);
            reflectionCam.cullingMask = reflectLayers;

            reflectionCam.transform.position = camPos;
            reflectionCam.transform.rotation = camRot;

            reflectionCam.targetTexture = targetTex;

            SetNearClipPlane();

            reflectionCam.Render();
        }

        void SetNearClipPlane()
        {
            reflectionCam.nearClipPlane = clipPlaneOffset;
        }
    }
}

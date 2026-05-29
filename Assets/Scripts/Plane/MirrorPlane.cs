using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;

namespace Camera
{
    // [ExecuteInEditMode]
    public class MirrorPlane : MonoBehaviour
    {
        private Renderer rend;

        public float clipPlaneOffset = 0.01f;
        public LayerMask reflectLayers = -1;

        public int recursionLimit = 2;

        private UnityEngine.Camera reflectionCam;
        private UnityEngine.Camera meinCampf;

        private RenderTexture renderTexture;
        private Material mirrorMaterial;

        private void Awake()
        {
            reflectionCam = GetComponent<UnityEngine.Camera>();

            rend = GetComponent<MeshRenderer>();
            mirrorMaterial = rend.material;

            CreateRenderTexture();
            CreateReflectionCamera();

            meinCampf = UnityEngine.Camera.main;
        }

        void CreateRenderTexture()
        {
            renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
            renderTexture.Create();
        }

        void CreateReflectionCamera()
        {
            GameObject camGO = new GameObject($"{name}_ReflectionCam")
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            reflectionCam = camGO.AddComponent<UnityEngine.Camera>();
            reflectionCam.enabled = false;
            reflectionCam.allowHDR = true;
            reflectionCam.targetTexture = renderTexture;
        }


        void LateUpdate()
        {
            RenderReflection(recursionLimit);
        }

        private static int currentFrame = -1;
        private int lastRenderFrame = -1;


        public void RenderReflection(int depth)
        {
            if (Time.frameCount != currentFrame)
            {
                currentFrame = Time.frameCount;
                // Сбрасываем флаги всех зеркал (можно хранить в списке, для простоты пропустим)
            }
            if (lastRenderFrame == Time.frameCount) return;
            lastRenderFrame = Time.frameCount;

            if (depth < 0) return;

            // Находим все зеркала в сцене
            MirrorPlane[] allMirrors = FindObjectsByType<MirrorPlane>(FindObjectsSortMode.None);

            if (depth == 0)
            {
                RenderSimpleReflection();
            }
            else
            {
                Material[] originalMaterials = new Material[allMirrors.Length];
                for (int i = 0; i < allMirrors.Length; i++)
                {
                    MirrorPlane m = allMirrors[i];
                    if (m == this) continue;

                    originalMaterials[i] = m.rend.sharedMaterial;

                    m.RenderReflection(depth - 1);

                    m.rend.material.mainTexture = m.renderTexture;
                }

                RenderSimpleReflection();

                for (int i = 0; i < allMirrors.Length; i++)
                {
                    MirrorPlane m = allMirrors[i];
                    if (m == this) continue;
                    m.rend.material = originalMaterials[i];
                }
            }

            mirrorMaterial.mainTexture = renderTexture;
        }

        void RenderSimpleReflection()
        {
            if (meinCampf == null) return;

            Vector3 mirrorNormal = transform.up;
            Vector3 mirrorPosition = transform.position;

            Vector3 reflectPos = ReflectPoint(meinCampf.transform.position, mirrorPosition, mirrorNormal);
            Vector3 reflectForward = ReflectVector(meinCampf.transform.forward, mirrorNormal);
            Vector3 reflectUp = ReflectVector(meinCampf.transform.up, mirrorNormal);

            reflectionCam.transform.position = reflectPos;
            reflectionCam.transform.LookAt(reflectPos + reflectForward, reflectUp);

            // // Настройка косой ближней плоскости отсечения
            // float d = -Vector3.Dot(mirrorNormal, mirrorPosition) - clipPlaneOffset;
            // Vector4 clipPlane = new Vector4(mirrorNormal.x, mirrorNormal.y, mirrorNormal.z, d);

            // var proj = reflectionCam.CalculateObliqueMatrix(clipPlane);
            // reflectionCam.projectionMatrix = proj;

            reflectionCam.fieldOfView = meinCampf.fieldOfView;
            reflectionCam.nearClipPlane = meinCampf.nearClipPlane;
            reflectionCam.farClipPlane = meinCampf.farClipPlane;
            reflectionCam.allowMSAA = meinCampf.allowMSAA;

            reflectionCam.cullingMask = reflectLayers;

            reflectionCam.clearFlags = meinCampf.clearFlags;
            reflectionCam.backgroundColor = meinCampf.backgroundColor;

            reflectionCam.targetTexture = renderTexture;
            reflectionCam.Render();

        }

        Vector3 ReflectPoint(Vector3 point, Vector3 planePos, Vector3 planeNormal)
        {
            float distance = Vector3.Dot(planeNormal, point - planePos);
            return point - 2 * distance * planeNormal;
        }

        Vector3 ReflectVector(Vector3 vec, Vector3 planeNormal)
        {
            return vec - 2 * Vector3.Dot(vec, planeNormal) * planeNormal;
        }
    }
}

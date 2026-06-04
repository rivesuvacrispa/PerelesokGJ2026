using System.Collections.Generic;
using Effects.EffectZones;
using Global;
using UnityEngine;

namespace Util
{
    public static class PhysUtils
    {
        private static Vector3[] allDirections = new[]
        {
            Vector3.forward,
            Vector3.left,
            -Vector3.forward,
            -Vector3.left,
            Vector3.down,
            Vector3.up,
        };

        public static bool TryFindAllMirrorsInRange(Vector3 origin, float distance, out List<Vector3> points)
        {
            points = new List<Vector3>(6);
            foreach (var d in allDirections)
            {
                Ray ray = new Ray(origin, d);
                bool intersect = Physics.Raycast(ray, out RaycastHit info, distance, 1 << GlobalDefinitions.MirrorsLayer);
                if (intersect) points.Add(info.point);
            }

            return points.Count != 0;
        }

        public static bool InsideEffectZone<T>(Vector3 origin, out int zonesCount) where T : SpellEffectZone
        {
            var colliders = Physics.OverlapSphere(origin, 0.1f, 1 << GlobalDefinitions.SpellEffectZoneLayer);
            zonesCount = 0;
            bool inside = false;
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<T>(out _))
                {
                    inside = true;
                    zonesCount++;
                }
            }

            return inside;
        }
    }
}
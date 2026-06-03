using System.Collections.Generic;
using UnityEngine;

namespace Story
{
    public class PlotManager : MonoBehaviour
    {
        private static readonly HashSet<PlotMemory> plotEvents = new();

        public static bool Has(PlotMemory plotEvent) => plotEvents.Contains(plotEvent);
        public static void Add(PlotMemory plotEvent) => plotEvents.Add(plotEvent);
    }
}
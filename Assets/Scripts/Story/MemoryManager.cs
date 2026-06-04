using System.Collections.Generic;
using UnityEngine;

namespace Story
{
    public class MemoryManager : MonoBehaviour
    {
        private static readonly HashSet<PlotMemory> PLOT_EVENTS = new();

        public static bool HasMemory(PlotMemory plotEvent) => PLOT_EVENTS.Contains(plotEvent);
        public static void AddMemory(PlotMemory plotEvent) => PLOT_EVENTS.Add(plotEvent);
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Plot
{
    public class PlotManager : MonoBehaviour
    {
        private static HashSet<PlotEvent> plotEvents;

        public static bool Has(PlotEvent plotEvent) => plotEvents.Contains(plotEvent);
        public static void Add(PlotEvent plotEvent) => plotEvents.Add(plotEvent);
    }
}
using UnityEngine;

namespace Story
{
    [CreateAssetMenu(menuName = "PlotMemory")]
    public class PlotMemory : ScriptableObject
    {
        [SerializeField] private string title;
    }
}
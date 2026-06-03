using UnityEngine;

namespace Plot
{
    [CreateAssetMenu(menuName = "PlotEvent")]
    public class PlotEvent : ScriptableObject
    {
        [SerializeField] private string title;
    }
}
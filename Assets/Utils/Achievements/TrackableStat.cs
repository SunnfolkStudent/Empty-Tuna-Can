using UnityEngine;

namespace Utils.Achievements
{
    [CreateAssetMenu(fileName = "TrackableStat", menuName = "Achievements/New TrackableStat")]
    public class TrackableStat : ScriptableObject
    {
        public int amount;
    }
}
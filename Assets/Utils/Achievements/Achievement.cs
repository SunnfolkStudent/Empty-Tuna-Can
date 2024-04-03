using UnityEngine;
using Utils.CustomAttributes;

namespace Utils.Achievements {
    [CreateAssetMenu(fileName = "Achievement", menuName = "Achievements/New Achievement")]
    public class Achievement : ScriptableObject {
        [Header("Info")]
        public string achievementName;
        public string achievementDescription;
        public bool isGotten;
        
        [Header("Requirements")] 
        [SerializeField] public Requirement[] requirements;
    }
    
    [System.Serializable]
    public class Requirement {
        public TrackableStat stat;
        public int amountRequired;
        public bool isComplete;
    }
}
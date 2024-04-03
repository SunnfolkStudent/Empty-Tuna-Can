using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.Achievements {
    public abstract class AchievementFunctions : MonoBehaviour {
        protected Achievement[] AllAchievements;
        protected TrackableStat[] AllTrackableStats;
        
        protected void Awake() => InitializeAchievements();
        
        private void InitializeAchievements() {
            AllAchievements = ScrubUtils.GetAllScrubsInResourceFolder<Achievement>("Achievements/AchievementScrubs");
            AllTrackableStats = ScrubUtils.GetAllScrubsInResourceFolder<TrackableStat>("Achievements/TrackableStatScrubs");
        }
        
        protected abstract void UnlockAchievement(Achievement achievement);
        
        protected void ResetAchievementProgress() {
            foreach (var trackableStat in AllTrackableStats) {
                trackableStat.amount = 0;
            }
            
            foreach (var achievement in AllAchievements) {
                achievement.isGotten = false;
                foreach (var requirement in achievement.requirements) {
                    requirement.isComplete = false;
                }
            }
        }

        protected void IncreaseTrackableStat(TrackableStat stat, int amount = 1) {
            stat.amount += amount;
            
            foreach (var achievement in AllAchievements) {
                if (achievement.isGotten || achievement.requirements.All(requirement => requirement.stat != stat)) continue;
                
                var matchingRequirement = achievement.requirements.First(requirement => requirement.stat == stat);
                if (matchingRequirement.stat.amount >= matchingRequirement.amountRequired) matchingRequirement.isComplete = true;
                
                if (!HasAllRequirements(achievement.requirements)) continue;
                achievement.isGotten = true;
                UnlockAchievement(achievement);
            }
        }
        
        private static bool HasAllRequirements(IEnumerable<Requirement> requirements)
        {
            return requirements.All(requirement => requirement.isComplete);
        }
    }
}
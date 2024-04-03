namespace Utils.Entity {
    public static class StatsFunctions {
        public static void LevelStat(ref ClampedFloat stat, float levelIncrement) {
            stat.maxValue += levelIncrement;
            stat.Value += levelIncrement; // RestoreStat(stat); // If desired full restore value on level up
        }
        
        public static void RestoreStat(ref ClampedFloat stat) {
            stat.Value = stat.maxValue;
        }
        
        public static void RestoreAllStats(ref ClampedFloat[] stats) {
            for (var index = 0; index < stats.Length; index++) {
                RestoreStat(ref stats[index]);
            }
        }
    }
}

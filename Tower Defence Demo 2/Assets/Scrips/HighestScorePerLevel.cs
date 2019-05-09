using Scrips.Data;

namespace Scrips
{
    [System.Serializable]
    public class HighestScorePerLevel
    {
        public LevelConfiguration Level;

        public IntCurrency HighestScore;
    }
}
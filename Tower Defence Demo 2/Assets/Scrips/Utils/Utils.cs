using Scrips.Waves;
using UnityEngine;

namespace Scrips.Utils
{
    public static class Utils
    {
        public static float GetDeviatedValue(float baseValue, float deviation, System.Random rand)
        {
            return baseValue * GetRandomNumber(1 - deviation, 1 + deviation, rand);
        }

        // origin: https://stackoverflow.com/questions/1064901/random-number-between-2-double-numbers
        public static float GetRandomNumber(float minimum, float maximum, System.Random rand = null)
        {
            if (rand == null) rand = new System.Random();
            return (float)rand.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
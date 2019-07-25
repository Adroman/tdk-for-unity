using System.Collections.Generic;
using Scrips.Data;
using Scrips.Data.Formula;
using Scrips.Skills;
using UnityEditor;
using UnityEngine;

namespace Scrips
{
    [CreateAssetMenu(menuName = "Tower defense kit/Player Data")]
    public class PlayerData : ScriptableObject
    {
        public static PlayerData ActivePlayer;

        public int PlayerLevel;

        public IntCurrency SkillPoints;

        public IntCurrency TotalScore;

        public IntCurrency TotalEnemiesKilled;

        public IntCurrency TotalTowersBuilt;

        public List<HighestScorePerLevel> CompletedLevels;

        public BaseFormula ScoreFormula;

        public List<PlayerSkill> Skills;

        public HighestScorePerLevel GetLevelOrDefault(LevelConfiguration level)
        {
            foreach (var completedLevel in CompletedLevels)
            {
                if (completedLevel.Level == level)
                    return completedLevel;
            }

            return new HighestScorePerLevel
            {
                Level = level,
                HighestScore = new IntCurrency
                {
                    Variable = TotalScore.Variable,
                    Amount = 0
                }
            };
        }

        public bool TryGetLevel(LevelConfiguration level, out HighestScorePerLevel output)
        {
            output = null;
            foreach (var completedLevel in CompletedLevels)
            {
                if (completedLevel.Level == level)
                {
                    output = completedLevel;
                    return true;
                }
            }

            return false;
        }

        public IntCurrency SetOrAddScore(LevelConfiguration level, IntCurrency newScore)
        {
            if(TryGetLevel(level, out var levelScore))
            {
                int oldAmount = levelScore.HighestScore.Amount;
                int newAmount = newScore.Amount;

                if (newAmount > oldAmount)
                {
                    int diff = newAmount - oldAmount;
                    levelScore.HighestScore.Amount = newAmount;

                    return new IntCurrency
                    {
                        Amount = diff,
                        Variable = newScore.Variable
                    };
                }
                else
                {
                    return new IntCurrency
                    {
                        Amount = 0,
                        Variable = newScore.Variable
                    };
                }
            }
            else
            {
                CompletedLevels.Add(
                    new HighestScorePerLevel
                    {
                        Level = level,
                        HighestScore = newScore
                    });
            }

            return newScore;
        }
    }
}
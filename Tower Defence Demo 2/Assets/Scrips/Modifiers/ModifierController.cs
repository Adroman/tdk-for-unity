using System;
using System.Collections.Generic;
using Scrips.Data;
using Scrips.EnemyData.Instances;
using Scrips.Modifiers.Enemies;
using Scrips.Modifiers.Towers;
using Scrips.Towers.BaseData;
using Scrips.Variables;
using UnityEngine;

namespace Scrips.Modifiers
{
    public class ModifierController : MonoBehaviour
    {
        [SerializeField]
        private List<BaseModifier> _allModifiers = new List<BaseModifier>();

        public IReadOnlyList<BaseModifier> Modifiers => _allModifiers;

        public TowerCollection Towers;

        public EnemyCollection Enemies;

        public void Start()
        {
            ImportSkills();
            
            foreach (var modifier in _allModifiers)
            {
                switch (modifier)
                {
                    case BaseTowerModifier towerModifier:
                        foreach (var tower in Towers.Instances)
                        {
                            towerModifier.AddToTower(tower);
                        }
                        break;
                    case BaseEnemyModifer enemyModifer:
                        foreach (var enemyInstance in Enemies.Instances)
                        {
                            enemyModifer.AddToEnemy(enemyInstance);
                        }
                        break;
                    case BaseEnemyCurrencyModifier enemyLootModifier:
                        foreach (var enemyInstance in Enemies.Instances)
                        {
                            enemyLootModifier.AddToEnemy(enemyInstance);
                        }
                        break;
                }
            }
        }

        private void ImportSkills()
        {
            var pd = PlayerData.ActivePlayer;

            foreach (var skill in pd.Skills)
            {
                foreach (var skillModifier in skill.Skill.Modifiers)
                {
                    skillModifier.Modifier.Level = skill.Level / Mathf.Max(1, skillModifier.PerLevelsApplied);
                    _allModifiers.Add(skillModifier.Modifier);
                }
            }
        }

        public void AddModifier(BaseModifier modifier)
        {
            _allModifiers.Add(modifier);

            switch (modifier)
            {
                case BaseTowerModifier towerModifier:
                    foreach (var tower in Towers.Instances)
                    {
                        towerModifier.AddToTower(tower);
                    }
                    break;
                case BaseEnemyModifer enemyModifer:
                    foreach (var enemyInstance in Enemies.Instances)
                    {
                        enemyModifer.AddToEnemy(enemyInstance);
                    }
                    break;
                case BaseEnemyCurrencyModifier enemyCurrencyModifier:
                    foreach (var enemyInstance in Enemies.Instances)
                    {
                        enemyCurrencyModifier.AddToEnemy(enemyInstance);
                    }
                    break;
                // add more stuff
            }
        }

        public void RemoveModifier(BaseModifier modifier)
        {
            if (_allModifiers.Remove(modifier))
            {
                switch (modifier)
                {
                    case BaseTowerModifier towerModifier:
                        foreach (var tower in Towers.Instances)
                        {
                            towerModifier.RemoveFromTower(tower);
                        }
                        break;
                    case BaseEnemyModifer enemyModifer:
                        foreach (var enemyInstance in Enemies.Instances)
                        {
                            enemyModifer.RemoveFromEnemy(enemyInstance);
                        }
                        break;
                    case BaseEnemyCurrencyModifier enemyLootModifier:
                        foreach (var enemyInstance in Enemies.Instances)
                        {
                            enemyLootModifier.RemoveFromEnemy(enemyInstance);
                        }
                        break;
                }
            }
        }

        public void ImportModifiers(TowerInstance tower)
        {
            foreach (var modifier in _allModifiers)
            {
                switch (modifier)
                {
                    case BaseTowerModifier towerModifier:
                        towerModifier.AddToTower(tower);
                        break;
                    // add more stuff
                }
            }
        }

        public void ImportModifiers(TowerUiData tower)
        {
            foreach (var modifier in _allModifiers)
            {
                switch (modifier)
                {
                    case BaseTowerModifier towerModifier:
                        towerModifier.AddToTower(tower);
                        break;
                    case TowerCostModifier towerCostModifier:
                        foreach (var price in tower.ModifiedPrice)
                        {
                            towerCostModifier.AddToTower(tower, price.Currency.Variable);
                        }
                        break;
                    // add more stuff
                }
            }
        }

        public void ImportModifiers(EnemyInstance enemy)
        {
            foreach (var modifier in _allModifiers)
            {
                switch (modifier)
                {
                    case BaseEnemyModifer enemyModifer:
                        enemyModifer.AddToEnemy(enemy);
                        break;
                    case BaseEnemyCurrencyModifier enemyCurrencyModifier:
                        enemyCurrencyModifier.AddToEnemy(enemy);
                        break;
                }
            }
        }
    }
}
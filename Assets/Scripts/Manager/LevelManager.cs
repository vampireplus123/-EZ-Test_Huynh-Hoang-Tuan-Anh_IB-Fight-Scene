using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level Settings")]
    [SerializeField] private int MaxLevel;
    [SerializeField] private int CurrentLevel = 0;
    public int[] DamageStatsEnemy;
    public int[] EnemyPawnInTheLevel;

    private List<Fighter> fighters = new List<Fighter>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterFighter(Fighter fighter)
    {
        if (!fighters.Contains(fighter))
            fighters.Add(fighter);
    }

    public void UnregisterFighter(Fighter fighter)
    {
        if (fighters.Contains(fighter))
            fighters.Remove(fighter);
    }

    public void AutoCreateLevel()
    {
        if (IsLevelMax()) return;

        CurrentLevel++;
    }

    public int GetEnemyDamageForLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < DamageStatsEnemy.Length)
        {
            return DamageStatsEnemy[levelIndex];
        }
        return 0;
    }

    public int GetCurrentLevel()
    {
        return CurrentLevel;
    }

    public bool IsLevelMax()
    {
        return CurrentLevel >= MaxLevel;
    }
}

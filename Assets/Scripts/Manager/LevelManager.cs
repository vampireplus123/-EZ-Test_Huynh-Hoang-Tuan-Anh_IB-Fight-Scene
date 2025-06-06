using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level Status")]
    [SerializeField] private int MaxLevel;
    [SerializeField] private int CurrentLevel;
    private List<Fighter> fighters = new List<Fighter>();
    public int[] DamageStatsEnemy;
    private int DamageForChanging = 0;
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
        {
            fighters.Add(fighter);
        }
    }

    public void UnregisterFighter(Fighter fighter)
    {
        if (fighters.Contains(fighter))
        {
            fighters.Remove(fighter);
        }
    }

    public void AutoCreateLevel()
    {
        CurrentLevel++;
        SetLevelStats();
    }

    public void SetLevelStats()
    {
        foreach (Fighter f in fighters)
        {
            if (f is EnemyController)
            {
                EnemyStats(f);
                Debug.Log("Is Enemy");
            }
        }
    }

    public int GetCurrentLevel()
    {
        return CurrentLevel;
    }

    public int GetMaxLevel()
    {
        return MaxLevel;
    }


    private void EnemyStats(Fighter fighters)
    {
        if (CurrentLevel < DamageStatsEnemy.Length)
        {
            DamageForChanging = DamageStatsEnemy[CurrentLevel];
        }
        fighters.SetDamage(DamageForChanging);
    }
}

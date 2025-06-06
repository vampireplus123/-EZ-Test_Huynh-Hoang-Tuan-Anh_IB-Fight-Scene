using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform spawnPoint;
    private List<Fighter> fighters = new List<Fighter>();

    public bool PlayerWon { get; private set; }
    public bool EnemyWon { get; private set; }
    public float delayForNextLevel = 5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SpawnEnemiesForLevel();
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

    public void OnFighterDeath(Fighter deadFighter)
    {
        UnregisterFighter(deadFighter);

        bool allEnemiesDead = true;
        bool playerDead = true;

        foreach (Fighter f in fighters)
        {
            if (f is EnemyController)
                allEnemiesDead = false;
            if (f is Player)
                playerDead = false;
        }

        if (allEnemiesDead)
        {
            PlayerWon = true;
            Debug.Log("Player WINS!");
            foreach (Fighter f in fighters)
            {
                if (f is Player)
                    f.Winner();
            }
        }
        else if (playerDead)
        {
            EnemyWon = true;
            Debug.Log("Enemy WINS!");
            foreach (Fighter f in fighters)
            {
                if (f is EnemyController)
                    f.Winner();
            }
        }

        GameStatusChange();
    }

    private void GameStatusChange()
    {
        if (PlayerWon)
        {
            StartCoroutine(NextLevelAfterDelay(delayForNextLevel));
        }
        else if (EnemyWon)
        {
            OverGame();
        }
    }

    private void SpawnEnemiesForLevel()
    {
        if (LevelManager.Instance.IsLevelMax())
        {
            Debug.Log(" Max level reached!");
            return;
        }

        LevelManager.Instance.AutoCreateLevel();

        int levelIndex = LevelManager.Instance.GetCurrentLevel() - 1;

        if (levelIndex < 0 || levelIndex >= LevelManager.Instance.EnemyPawnInTheLevel.Length)
        {
            Debug.LogWarning("Current level index out of bounds.");
            return;
        }

        int enemyCount = LevelManager.Instance.EnemyPawnInTheLevel[levelIndex];
        int enemyDamage = LevelManager.Instance.GetEnemyDamageForLevel(levelIndex);

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyObj = EnemyPool.Instance.GetEnemy();
            enemyObj.transform.position = spawnPoint.position;

            Fighter enemyFighter = enemyObj.GetComponent<Fighter>();
            enemyFighter.SetDamage(enemyDamage);

            RegisterFighter(enemyFighter);
            LevelManager.Instance.RegisterFighter(enemyFighter);
        }

        PlayerWon = false;
        EnemyWon = false;
    }

    private IEnumerator NextLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnEnemiesForLevel();
    }

    private void OverGame()
    {
        Debug.Log("Game Over! Enemy wins!");
    }
}
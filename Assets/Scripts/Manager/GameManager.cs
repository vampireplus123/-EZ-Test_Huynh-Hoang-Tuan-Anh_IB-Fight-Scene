using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    private List<Fighter> fighters = new List<Fighter>();

    public bool PlayerWon { get; private set; }
    public bool EnemyWon { get; private set; }

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnEnemy();
        }
    }

    // Đăng ký fighter vào danh sách quản lý
    public void RegisterFighter(Fighter fighter)
    {
        if (!fighters.Contains(fighter))
        {
            fighters.Add(fighter);
        }
    }

    // Hủy đăng ký fighter (ví dụ khi fighter chết hoặc thoát game)
    public void UnregisterFighter(Fighter fighter)
    {
        if (fighters.Contains(fighter))
        {
            fighters.Remove(fighter);
        }
    }

    // Xử lý khi một fighter chết
    public void OnFighterDeath(Fighter deadFighter)
    {
        // Xóa fighter chết khỏi danh sách quản lý
        UnregisterFighter(deadFighter);

        // Nếu còn fighter sống, đánh dấu họ là người thắng
        foreach (Fighter f in fighters)
        {
            f.Winner();

            string winnerName = "Unknown";
            if (f is Player)
            {
                winnerName = "Player";
                PlayerWon = true;
                EnemyWon = false;
            }
            else if (f is EnemyController)
            {
                winnerName = "Enemy";
                EnemyWon = true;
                PlayerWon = false;
            }

            Debug.Log($"{winnerName} WINS!");
        }

        // Kiểm tra trạng thái game sau khi có người thắng
        GameStatusChange();
    }

    // Kiểm tra trạng thái game, xử lý tiếp theo
    public void GameStatusChange()
    {
        if (PlayerWon)
        {
            // Tăng level nếu chưa đạt max
            LevelManager.Instance.AutoCreateLevel();

            // Có thể spawn enemy mới nếu level còn tiếp tục
            // SpawnEnemyIfLevelStillRemain();
        }
        else if (EnemyWon)
        {
            OverGame();
        }
    }

    // Hàm để spawn enemy khi level còn chưa kết thúc
    private void SpawnEnemyIfLevelStillRemain()
    {
        if (LevelManager.Instance.GetCurrentLevel() < LevelManager.Instance.GetMaxLevel())
        {
            Debug.Log("Spawning enemy for next level...");
            SpawnEnemy();
        }
        else
        {
            Debug.Log("All levels cleared! Player wins the game!");
            OverGame();
        }
    }
    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        Fighter enemyFighter = enemy.GetComponent<Fighter>();

        // Set chỉ số damage theo level
        int currentLevel = LevelManager.Instance.GetCurrentLevel();
        int newDamage = (currentLevel < LevelManager.Instance.DamageStatsEnemy.Length)
            ? LevelManager.Instance.DamageStatsEnemy[currentLevel]
            : 10;

        enemyFighter.SetDamage(newDamage);

        // Đăng ký enemy mới
        LevelManager.Instance.RegisterFighter(enemyFighter);
        RegisterFighter(enemyFighter);
    }


    // Xử lý khi game kết thúc (ví dụ player thua)
    public void OverGame()
    {
        Debug.Log("Game Over! Enemy wins!");
        // TODO: Hiển thị màn hình kết thúc, reset game hoặc menu
    }
}

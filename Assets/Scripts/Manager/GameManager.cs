using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private List<Fighter> fighters = new List<Fighter>();

    public bool PlayerWon;
    public bool EnemyWon;

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

    public void OnFighterDeath(Fighter deadFighter)
    {
        foreach (Fighter f in fighters)
        {
            if (f != deadFighter)
            {
                f.Winner();

                string winnerName = "Unknown";
                if (f is Player)
                {
                    PlayerWon = true;
                    winnerName = "Player";
                }
                else if (f is EnemyController)
                {
                    EnemyWon = true;
                    winnerName = "Enemy";
                }

                Debug.Log($"{winnerName} WINS!");
            }
        }
    }
    public void GameStatusChange()
    {
        if (PlayerWon)
        {
            //Check trong LevelManager con bn man choi
            //Neu con thi choi tiep
            //Neu trong Level cuoi cung ma than thi over game
        }
        if (EnemyWon)
        {
            //Over Game:
            //Hien Panel diem
            return;
        }
    }

    public void OverGame()
    {
        //Game over    
    }
    
}

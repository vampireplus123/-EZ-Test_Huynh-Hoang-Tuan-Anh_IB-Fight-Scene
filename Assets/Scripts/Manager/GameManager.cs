using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
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

    public void OnFighterDeath(Fighter deadFighter)
    {
        foreach (Fighter f in fighters)
        {
            if (f != deadFighter)
            {
                f.Winner();

                string winnerName = "Unknown";
                if (f is Player)
                    winnerName = "Player";
                else if (f is EnemyController)
                    winnerName = "Enemy";

                Debug.Log($"{winnerName} WINS!");
            }
        }
    }
    
}

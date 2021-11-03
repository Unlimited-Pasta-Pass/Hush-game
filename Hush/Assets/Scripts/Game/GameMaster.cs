using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;   

public class GameMaster : MonoBehaviour
{
    public static bool playerHasRelic;
    public static int keysInPossession;

    private static HashSet<int> enemiesAttacking;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    void Start()
    {
        playerHasRelic = false;
        keysInPossession = 0;
    }

    public static void CollectKey()
    {
        keysInPossession += 1;
    }

    public static void HasRelic(bool inPossession)
    {
        playerHasRelic = inPossession;
    }

    public static void AddToEnemyList(int id)
    {
        enemiesAttacking.Add(id);
    }
    
    public static void RemoveFromEnemyList(int id)
    {
        enemiesAttacking.Remove(id);
    }

    public static bool IsPlayerInCombat()
    {
        return enemiesAttacking.Count > 0;
    }
}
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] GameObject keyCounter;
    public static bool playerHasRelic;
    public static int keysInPossession;

    private static HashSet<int> enemiesAttacking;
    private static KeyCounterUI keyCountUI;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        enemiesAttacking = new HashSet<int>();
        keyCountUI = keyCounter.GetComponent<KeyCounterUI>();
    }
    
    void Start()
    {
        playerHasRelic = false;
        keysInPossession = 0;
        keyCountUI.SetKeySpriteVisibility(keysInPossession > 0);
        keyCountUI.UpdateKeyCounter();
    }

    public static void CollectKey()
    {
        keysInPossession += 1;
        keyCountUI.SetKeySpriteVisibility(keysInPossession > 0);
        keyCountUI.UpdateKeyCounter();
    }
    public static void ResetKeys () {
        keysInPossession = 0;
    }

    public static void RelicCollect () {
        if (!playerHasRelic)
        {
            playerHasRelic = true;
        }
        else
        {
            Debug.Log("Player cannot have more than 1 relic");
        }
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
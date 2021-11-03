using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState instance;

    public bool playerEngagedInCombat;
    public bool playerHasRelic;
    public int keysInPossession;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    void Start()
    {
        playerEngagedInCombat = false;
        playerHasRelic = false;
        keysInPossession = 0;
    }

    public void CollectKey()
    {
        keysInPossession += 1;
    }

    public void IsEngagedInCombat(bool inCombat)
    {
        playerEngagedInCombat = inCombat;
    }
}

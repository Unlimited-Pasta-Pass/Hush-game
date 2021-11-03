using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static bool playerEngagedInCombat;
    public static bool playerHasRelic;
    public static int keysInPossession;

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

    public static void CollectKey()
    {
        keysInPossession += 1;
    }

    public static void IsEngagedInCombat(bool inCombat)
    {
        playerEngagedInCombat = inCombat;
    }

    public static void HasRelic(bool inPossession)
    {
        playerHasRelic = inPossession;
    }
}
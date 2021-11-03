using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState instance;

    public bool engagedInCombat;
    public bool hasRelic;

    void Start()
    {
        engagedInCombat = false;
        hasRelic = false;
    }
}

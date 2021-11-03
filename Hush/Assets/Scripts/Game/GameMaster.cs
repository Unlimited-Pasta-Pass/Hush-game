using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;

    private static int numOfKeys = 0;
    public static int NumOfKeys {
        get {
            return numOfKeys;
        }
    }
    private static bool hasRelic = false;
    public static bool HasRelic {
        get {
            return hasRelic;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (gm == null) {
            gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
        }
    }

    public static void KeyCollect (Key key) {
        numOfKeys += 1;
    }
}

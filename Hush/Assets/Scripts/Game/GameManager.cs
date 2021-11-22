using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GameManager : MonoBehaviour
{
    // States
    private bool _playerHasRelic;
    private int _keysInPossession;
    private HashSet<int> _enemiesAttacking;

    // Events
    public UnityEvent<int> keyCountChanged;

    // Instance
    public static GameManager Instance;

    // Getters
    public bool IsPlayerInCombat => _enemiesAttacking.Count > 0;
    public bool PlayerHasRelic => _playerHasRelic;
    public int KeysInPossession => _keysInPossession;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        DontDestroyOnLoad(Instance.gameObject);

        Instance.InitializeValues();
    }

    private void InitializeValues()
    {
        _playerHasRelic = false;
        _keysInPossession = 0;
        _enemiesAttacking = new HashSet<int>();

        keyCountChanged ??= new UnityEvent<int>();
        
        // Make sure the UI follows the Game Master State
        keyCountChanged.Invoke(_keysInPossession);
    }

    // Keys
    public void CollectKey()
    {
        _keysInPossession += 1;
        keyCountChanged.Invoke(_keysInPossession);
    }
    
    public void ResetKeys () {
        _keysInPossession = 0;
    }

    // Relic
    public void CollectRelic () {
        if (!_playerHasRelic)
        {
            _playerHasRelic = true;
        }
        else
        {
            Debug.Log("Player cannot have more than 1 relic");
        }
    }
    
    public void ResetRelic () 
    {
        _playerHasRelic = false;
    }

    // Enemies
    public void AddToEnemyList(int id)
    {
        _enemiesAttacking.Add(id);
    }
    
    public void RemoveFromEnemyList(int id)
    {
        _enemiesAttacking.Remove(id);
    }
}
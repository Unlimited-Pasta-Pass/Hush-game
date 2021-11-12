using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GameManager : MonoBehaviour
{
    // Instance
    public static GameManager Instance;
    
    #region Game States

    private float _playerHitPoints;
    
    private bool _playerHasRelic;
    private int _keysInPossession;
    private HashSet<int> _enemiesAttacking;

    private int _currentlyLoadedScene;
    private Dictionary<int, bool> _keySpawnersInUse;
    
    #endregion

    #region Events
    
    public UnityEvent<int> keyCountChanged;

    #endregion
    
    #region Public Getters

    public float PlayerHitPoints => _playerHitPoints;
    
    public bool IsPlayerInCombat => _enemiesAttacking.Count > 0;
    public bool PlayerHasRelic => _playerHasRelic;
    public int KeysInPossession => _keysInPossession;

    public int CurrentlyLoadedScene => _currentlyLoadedScene;

    private ReadOnlyDictionary<int, bool> _readOnlyDictionary;
    public ReadOnlyDictionary<int, bool> KeySpawnersInUse => _readOnlyDictionary ??= new ReadOnlyDictionary<int, bool>(_keySpawnersInUse);

    #endregion
    
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
        _enemiesAttacking ??= new HashSet<int>();

        _currentlyLoadedScene = 0;
        _keySpawnersInUse ??= new Dictionary<int, bool>();

        keyCountChanged ??= new UnityEvent<int>();
        
        // Make sure the UI follows the Game Master State
        keyCountChanged.Invoke(_keysInPossession);
    }

    // Player
    public void SetPlayerHitPoints(float hp)
    {
        _playerHitPoints = hp;
    }
    public bool UpdatePlayerHitPoints(float hpDelta)
    {
        _playerHitPoints += hpDelta;

        if (_playerHitPoints > 0) 
            return true;
        
        _playerHitPoints = 0;
        return false;
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
        _playerHasRelic = true;
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

    public void UpdateKeySpawnerList(IEnumerable<int> spawners)
    {
        foreach (var spawner in spawners)
        {
            _keySpawnersInUse.Add(spawner, false);
        }
    }
    
    public void UseKeySpawner(int instanceId)
    {
        _keySpawnersInUse[instanceId] = true;
    }
}
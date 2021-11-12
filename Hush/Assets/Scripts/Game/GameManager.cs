using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Game.Models;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GameManager : MonoBehaviour
{
    // Instance
    public static GameManager Instance;
    
    public UnityEvent<int> keyCountChanged;

    private GameModel _game;

    #region Public Getters

    public GameModel CurrentGame => _game;
    public float PlayerCurrentHitPoints => _game.playerCurrentHitPoints;
    public float PlayerMaxHitPoints => _game.playerMaxHitPoints;
    public bool IsPlayerInCombat => _game.enemiesAttacking.Count > 0;
    public bool PlayerHasRelic => _game.playerHasRelic;
    public int KeysInPossession => _game.keysInPossession;
    public int CurrentlyLoadedScene => _game.currentlyLoadedScene;

    private ReadOnlyDictionary<int, bool> _readOnlyDictionary;
    public ReadOnlyDictionary<int, bool> KeySpawnersInUse => _readOnlyDictionary ??= new ReadOnlyDictionary<int, bool>(_game.keySpawnersInUse);

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
        _game ??= new GameModel
        {
            playerCurrentHitPoints = 0f,
            playerMaxHitPoints = 0f,
            playerHasRelic = false,
            keysInPossession = 0,
            enemiesAttacking = new HashSet<int>(),
            currentlyLoadedScene = 0,
            keySpawnersInUse = new Dictionary<int, bool>()
        };

        keyCountChanged ??= new UnityEvent<int>();
        
        // Make sure the UI follows the Game Master State
        keyCountChanged.Invoke(_game.keysInPossession);
    }

    // Game Model
    public void SetGameModel(GameModel gameModel)
    {
        _game = gameModel;
    }
    
    // Player
    public void SetPlayerMaxHitPoints(float hp)
    {
        _game.playerMaxHitPoints = hp;
    }
    public void SetPlayerHitPoints(float hp)
    {
        _game.playerCurrentHitPoints = hp;
    }
    public bool UpdatePlayerHitPoints(float hpDelta)
    {
        _game.playerCurrentHitPoints += hpDelta;

        if (_game.playerCurrentHitPoints > 0) 
            return true;
        
        _game.playerCurrentHitPoints = 0;
        return false;
    }
    
    // Keys
    public void CollectKey()
    {
        _game.keysInPossession += 1;
        keyCountChanged.Invoke(_game.keysInPossession);
    }
    
    public void ResetKeys () {
        _game.keysInPossession = 0;
    }

    // Relic
    public void CollectRelic () {
        _game.playerHasRelic = true;
    }
    
    public void ResetRelic () 
    {
        _game.playerHasRelic = false;
    }

    // Enemies
    public void AddToEnemyList(int id)
    {
        _game.enemiesAttacking.Add(id);
    }
    
    public void RemoveFromEnemyList(int id)
    {
        _game.enemiesAttacking.Remove(id);
    }

    public void UpdateKeySpawnerList(IEnumerable<int> spawners)
    {
        foreach (var spawner in spawners)
        {
            _game.keySpawnersInUse.Add(spawner, false);
        }
    }
    
    public void UseKeySpawner(int instanceId)
    {
        _game.keySpawnersInUse[instanceId] = true;
    }
}
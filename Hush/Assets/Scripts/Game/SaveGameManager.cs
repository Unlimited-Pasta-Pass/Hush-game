﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Game.Models;
using UnityEngine;

namespace Game
{
    public class SaveGameManager : MonoBehaviour
    {
        public static SaveGameManager Instance;

        public bool HasSavedGame => File.Exists(_path);
        
        public int SavedGameScene => OnLoad(false).currentlyLoadedScene;
        
        private string _path = "/pasta.save";
        
        private BinaryFormatter _formatter;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            
            DontDestroyOnLoad(Instance.gameObject);
            
            _path = Application.persistentDataPath + _path;
            _formatter ??= new BinaryFormatter();
        }

        public bool OnSave()
        {
            try
            {
                GameManager.Instance.SetSaveTime(Time.time);

                var stream = new FileStream(_path, FileMode.Create);
                _formatter.Serialize(stream, GameManager.Instance.CurrentGameState);
                stream.Close();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
        
        public GameState OnLoad(bool applySavedGames = true)
        {
            if (!HasSavedGame)
                return null;
            
            var stream = new FileStream(_path, FileMode.Open);
            var model = _formatter.Deserialize(stream) as GameState;
            stream.Close();
            
            if (applySavedGames)
                GameManager.Instance.SetGameState(model);

            return model;
        }
    }
}
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Game.Models;
using UnityEngine;

namespace Game
{
    public class SaveGameManager : MonoBehaviour
    {
        public static SaveGameManager Instance;
        
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

        public void OnSave()
        {
            var stream = new FileStream(_path, FileMode.Create);
            _formatter.Serialize(stream, GameManager.Instance.CurrentGameState);
            stream.Close();
        }
        
        public void OnLoad()
        {
            if (!File.Exists(_path))
                return;
            
            var stream = new FileStream(_path, FileMode.Open);
            var model = _formatter.Deserialize(stream) as GameState;
            stream.Close();
            
            GameManager.Instance.SetGameState(model);
        }
    }
}
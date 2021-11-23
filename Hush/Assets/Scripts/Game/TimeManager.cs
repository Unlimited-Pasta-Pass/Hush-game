using UnityEngine;

namespace Game
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance;

        private bool _gameIsPaused;
        private float _prevTimeScale;
    
        public bool GameIsPaused => _gameIsPaused;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            
            DontDestroyOnLoad(Instance.gameObject);
        }

        public void Pause()
        {
            _prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            _gameIsPaused = true;
        }

        public void Resume()
        {
            Time.timeScale = _prevTimeScale;
            _prevTimeScale = 0f;
            _gameIsPaused = false;
        }

        public void Reset()
        {
            Time.timeScale = 1f;
            _prevTimeScale = 0f;
            _gameIsPaused = false;
        }
    }
}
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    private bool mGameIsPaused;

    private float mPrevTimeScale;
    private float mPrevDeltaTime = 0f;


    private void Awake()
    {
        Instance = this;
    }

    public bool GameIsPaused()
    {
        return mGameIsPaused;
    }

    public void Pause()
    {
        mPrevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        mGameIsPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = mPrevTimeScale;
        mPrevTimeScale = 0f;
        mGameIsPaused = false;
    }

    public void Reset()
    {
        Time.timeScale = 1f;
        mPrevTimeScale = 0f;
        mGameIsPaused = false;
    }
}
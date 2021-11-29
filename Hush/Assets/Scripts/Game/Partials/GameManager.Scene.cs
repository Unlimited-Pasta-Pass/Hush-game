namespace Game
{
    public partial class GameManager
    {
        public int CurrentlyLoadedScene => _state.currentlyLoadedScene;

        public void SetLoadedScene(int currentlyLoadedScene)
        {
            _state.currentlyLoadedScene = currentlyLoadedScene;
        }
    }
}
using UnityEngine;

namespace Manager
{
    public class GameButtonManagers : MonoBehaviour
    {
        public void NewGame()
        {
            Debug.Log("New");
            GameManagers.Instance.StartNewGame("Main");
        }

        public void ContinueGame()
        {
            Debug.Log("ContinueGame");
            GameManagers.Instance.ContinueGame();
        }

        public void Home()
        {
            GameManagers.Instance.ExitToTitle();
        }

        public void ExitGame()
        {
            GameManagers.Instance.ExitGame();
        }
    }
}
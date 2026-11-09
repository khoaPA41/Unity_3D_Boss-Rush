using UnityEngine;

namespace Managers
{
    public class MenuButtons : MonoBehaviour
    {
        public void NewGame()
        {
            GameManager.Instance.StartNewGame("Main");
        }

        public void ContinueGame()
        {
            GameManager.Instance.ContinueGame();
        }

        public void Home()
        {
            GameManager.Instance.ExitToTitle();
        }

        public void ExitGame()
        {
            GameManager.Instance.ExitGame();
        }
    }
}
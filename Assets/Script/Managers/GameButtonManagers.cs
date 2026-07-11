using UnityEngine;

public class GameButtonManagers : MonoBehaviour
{
    public void NewGame()
    {
        GameManagers.Instance.StartNewGame("Main");
    }

    public void ContinueGame()
    {
        GameManagers.Instance.ContinueGame();
    }

    public void ExitGame()
    {
        GameManagers.Instance.ExitGame();
    }
}
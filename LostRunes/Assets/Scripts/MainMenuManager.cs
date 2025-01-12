using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
        UIManager.Instance.StartGame();
    }

    public void OnCreditButtonClicked()
    {
        UIManager.Instance.GoToCredits();
    }
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
}


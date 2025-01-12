using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Arrastra aquí el Canvas del menú de pausa.
    private bool isPaused = false;

    void Update()
    {
        // Detectar la tecla de pausa (Esc)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.TogglePause();
        }
    }

    public void LoadMainMenu()
    {
        UIManager.Instance.GoToMenuScene();
    }

    public void ResumeGame()
    {
        UIManager.Instance.TogglePause();
    }
}


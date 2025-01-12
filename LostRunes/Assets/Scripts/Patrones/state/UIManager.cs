using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Singleton
    public static UIManager Instance { get; private set; }

    public enum UIStates
    {
        MainMenu,
        GameScene,
        Defeat,
        Victory,
        Credits,
        Pause
    }

    private UIStates currentState;

    private readonly string[] sceneNames = {
        "MenuScene",
        "GameScene",
        "DefeatScene",
        "VictoryScene",
        "CreditsScene"
    };

    public GameObject pauseMenuPrefab; // Prefab del menú de pausa


    private GameObject pauseMenuInstance; // Instancia activa del menú de pausa


    private void Awake()
    {
        // Configurar Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        ChangeState(UIStates.GameScene);
    }

    public void GoToCredits()
    {
        ChangeState(UIStates.Credits);
    }

    public void GoToMenuScene()
    {
        ChangeState(UIStates.MainMenu);
    }

    public void GoToVictory()
    {
        ChangeState(UIStates.Victory);
    }

    public void GoToDefeat()
    {
        ChangeState(UIStates.Defeat);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void TogglePause()
    {
        if (currentState == UIStates.Pause)
        {
            ResumeGame();
        }
        else if (currentState == UIStates.GameScene)
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        // Cambiar al estado de pausa
        currentState = UIStates.Pause;

        // Si no existe una instancia, instanciar el prefab
        if (pauseMenuInstance == null && pauseMenuPrefab != null)
        {
            pauseMenuInstance = Instantiate(pauseMenuPrefab);
            pauseMenuInstance.SetActive(false); // Asegúrate de que inicie desactivado
        }

        if (pauseMenuInstance != null)
        {
            Debug.Log("Activando Canvas de Pausa");
            pauseMenuInstance.SetActive(true); // Mostrar el menú de pausa
        }
        else
        {
            Debug.LogWarning("El prefab del menú de pausa no está asignado.");
        }

        // Pausar el tiempo en el juego
        Time.timeScale = 0f;
    }



    private void ResumeGame()
    {
        // Volver al estado de juego
        currentState = UIStates.GameScene;

        // Desactivar el menú de pausa si existe
        if (pauseMenuInstance != null)
        {
            pauseMenuInstance.SetActive(false);
        }

        // Reanudar el tiempo en el juego
        Time.timeScale = 1f;
    }


    public void ChangeState(UIStates newState)
    {
        if (currentState == newState) return;

        UnloadCurrentState();
        currentState = newState;
        LoadStateScene(currentState);
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (pauseMenuInstance != null)
        {
            Destroy(pauseMenuInstance);
            pauseMenuInstance = null;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }


    private void UnloadCurrentState()
    {
        // Si el estado no requiere descargar una escena, salimos.
        if (currentState == UIStates.Pause)
        {
            return;
        }

        string currentSceneName = sceneNames[(int)currentState];

        // Asegurarse de que la escena esté cargada antes de intentar descargarla
        if (SceneManager.GetSceneByName(currentSceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(currentSceneName);
        }
    }

    private void LoadStateScene(UIStates state)
    {
        if (state == UIStates.Pause)
        {
            return;
        }

        string sceneName = sceneNames[(int)state];

        if (state == UIStates.GameScene)
        {
            // Asegúrate de que la escena de GameScene reemplace la actual
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        else
        {
            // Cargar otras escenas en modo aditivo
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}

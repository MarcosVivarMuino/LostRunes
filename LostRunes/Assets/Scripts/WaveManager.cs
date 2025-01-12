using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public int totalWaves = 10;           // Número total de oleadas
    public float timeBetweenWaves = 5f;  // Tiempo entre oleadas
    public float spawnRadius = 10f;      // Radio para los puntos de aparición

    [SerializeField] private EnemyPool enemyPool; // Pool de enemigos
    [SerializeField] private EnemySpawner enemySpawner; // Referencia al EnemySpawner
    [SerializeField] private Slider waveProgressSlider; // Referencia al slider de progreso
    [SerializeField] private TextMeshProUGUI countdownText; // Texto de cuenta regresiva antes de la oleada

    private Transform towerTransform;            // Posición de la torre
    private int currentWave = 0;                 // Contador de la oleada actual
    private int enemiesRemaining = 0;           // Enemigos restantes en la oleada actual
    private int totalEnemiesInWave = 0;         // Total de enemigos en la oleada actual
    private int enemiesKilledThisWave = 0;      // Enemigos eliminados en la oleada actual
    private int totalEnemiesInGame = 0;         // Total de enemigos en todas las oleadas

    void Start()
    {
        // Obtener la posición de la torre
        towerTransform = GameObject.FindGameObjectWithTag("Tower").transform;

        if (enemyPool == null)
        {
            Debug.LogError("EnemyPool no asignado al WaveManager.");
        }

        if (enemySpawner == null)
        {
            Debug.LogError("EnemySpawner no asignado al WaveManager.");
        }

        if (waveProgressSlider == null)
        {
            Debug.LogError("Slider de progreso no asignado al WaveManager.");
        }
        else
        {
            waveProgressSlider.value = 0;
            waveProgressSlider.maxValue = 1;
        }

        if (countdownText == null)
        {
            Debug.LogError("Texto de cuenta regresiva no asignado al WaveManager.");
        }
        else
        {
            countdownText.gameObject.SetActive(false); // Ocultar el texto al inicio
        }

        // Calcular el total de enemigos en todas las oleadas
        for (int i = 1; i <= totalWaves; i++)
        {
            totalEnemiesInGame += 5 + (1 * i); // Ajustar según el cálculo de enemigos por oleada
        }

        // Iniciar la secuencia de oleadas
        StartCoroutine(StartWaveSequence());
    }

    private IEnumerator StartWaveSequence()
    {
        while (currentWave < totalWaves)
        {
            // Incrementar la oleada
            currentWave++;
            Debug.Log($"Preparando oleada {currentWave}.");

            // Mostrar el mensaje de cuenta regresiva
            yield return StartCoroutine(ShowWaveCountdown(currentWave));

            // Calcular el número de enemigos
            totalEnemiesInWave = 5 + (1 * currentWave);
            enemiesRemaining = totalEnemiesInWave;
            enemiesKilledThisWave = 0;

            // Generar enemigos
            enemySpawner.SpawnEnemies(totalEnemiesInWave, this);

            // Esperar hasta que todos los enemigos sean derrotados
            while (enemiesRemaining > 0)
            {
                yield return null;
            }

            Debug.Log($"Oleada {currentWave} completada.");

            // Esperar un tiempo antes de la siguiente oleada
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("¡Todas las oleadas completadas! Victoria.");
        EndGame();
    }

    private IEnumerator ShowWaveCountdown(int waveNumber)
    {
        // Activar el texto de cuenta regresiva
        countdownText.gameObject.SetActive(true);
        countdownText.text = $"Inicio de la Oleada {waveNumber}";
        yield return new WaitForSeconds(1f);
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = $"{i}";
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = $"¡Inicio!";
        yield return new WaitForSeconds(1f);

        // Ocultar el texto cuando la oleada comience
        countdownText.gameObject.SetActive(false);
    }

    public void HandleEnemyDeath()
    {
        enemiesRemaining--;
        enemiesKilledThisWave++;

        // Actualizar el slider de progreso
        if (waveProgressSlider != null)
        {
            float progressPerEnemy = 1f / totalEnemiesInGame; // Porcentaje que representa cada enemigo
            waveProgressSlider.value += progressPerEnemy; // Incrementar progreso global
        }

        if (enemiesRemaining <= 0)
        {
            Debug.Log("Todos los enemigos de la oleada han sido derrotados.");
        }
    }

    private void EndGame()
    {
        UIManager.Instance.GoToVictory();
    }
}
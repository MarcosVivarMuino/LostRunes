using UnityEngine;
using System.Collections.Generic;

using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs; // Prefabs de enemigos
    [SerializeField] private int poolSize = 10;         // Tamaño del pool por tipo

    private Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();

    void Start()
    {
        foreach (GameObject prefab in enemyPrefabs)
        {
            string prefabKey = prefab.name; // Usar el nombre del prefab como clave
            Queue<GameObject> enemyQueue = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject enemy = Instantiate(prefab);
                enemy.SetActive(false);
                enemy.transform.SetParent(transform);
                enemyQueue.Enqueue(enemy);
            }

            pools[prefabKey] = enemyQueue;
        }
    }

    public GameObject Get()
    {
        string randomKey = GetRandomEnemyKey();

        if (pools.ContainsKey(randomKey) && pools[randomKey].Count > 0)
        {
            GameObject enemy = pools[randomKey].Dequeue();
            return enemy;
        }
        else
        {
            Debug.LogError($"No hay enemigos disponibles en el pool para la clave '{randomKey}'.");
            return null;
        }
    }

    public void Return(GameObject enemy)
    {
        // Asegurarse de que el nombre no tenga "(Clone)"
        string enemyKey = enemy.name.Replace("(Clone)", "").Trim();

        if (pools.ContainsKey(enemyKey))
        {
            enemy.SetActive(false);
            enemy.transform.SetParent(transform);
            pools[enemyKey].Enqueue(enemy);
        }
        else
        {
            Debug.LogWarning($"El enemigo con clave '{enemyKey}' no está registrado en el pool. Destruyendo el objeto.");
            Destroy(enemy); // Destruir si no pertenece al pool
        }
    }

    private string GetRandomEnemyKey()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        return enemyPrefabs[index].name; // Devolver el nombre del prefab como clave
    }
}
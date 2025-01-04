using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy meleeEnemyPrototype;   // Prototipo de enemigo melee
    public Enemy rangedEnemyPrototype;  // Prototipo de enemigo a distancia

    public void SpawnEnemy(Vector3 position, bool isRanged)
    {
        Enemy enemyClone;

        if (isRanged)
        {
            enemyClone = rangedEnemyPrototype.Clone(); // Clonar prototipo
        }
        else
        {
            enemyClone = meleeEnemyPrototype.Clone();
        }

        // Posicionar al enemigo clonado
        enemyClone.transform.position = position;
    }
}

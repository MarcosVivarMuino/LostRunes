using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public GameObject projectilePrefab; // Proyectil que dispara

    public override Enemy Clone()
    {
        return Instantiate(this);
    }

    // Sobrescribir para disparar proyectiles
    public void Shoot(Vector3 target)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector3 direction = (target - transform.position).normalized;
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 10f;
    }
}

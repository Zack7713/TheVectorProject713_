using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotation : MonoBehaviour
{
    [Header("-----Attributes-----")]
    [SerializeField] float range;
    [SerializeField] float fireRate;
    private float fireCountDown = 0;

    [Header("-----Enemy-----")]
    [SerializeField] Transform target;
    [SerializeField] string enemyTag = "Enemy";

    [Header("-----Bullet-----")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;

    [Header("-----Rotation-----")]
    [SerializeField] Transform pivot;
    [SerializeField] float turnSpeed;
    

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void Update()
    {
        if (target == null) 
        {
            return;
        }

        // Target lock on
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(pivot.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        pivot.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if(fireCountDown <= 0)
        {
            Shoot();
            fireCountDown = 1 / fireRate;
        }

        fireCountDown -= Time.deltaTime;
    }
    void UpdateTarget()
    {
        // Puts all enemies with the same tag into array
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // Calculate the closest enemy to turret location
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance) 
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        // If enemy is within range, assign it as a target
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else 
            target = null;
    }

    void Shoot()
    {
        // Instantiate the bullet object and get the script from the bullet object
        GameObject turretBullet = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        TurretBullet bullet = turretBullet.GetComponent<TurretBullet>();

        // If we made a bullet, call the bullets function
        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visually show max range in scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pivot.position, range);
    }
}



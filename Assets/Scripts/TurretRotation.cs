using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotation : MonoBehaviour
{
    [Header("-----General-----")]
    [SerializeField] float range;
    [SerializeField] GameObject sphereRange;
    //private Enemy targetEnemy;

    [Header("-----Rotation-----")]
    [SerializeField] float turnSpeed;
    [SerializeField] Transform pivot;

    [Header("-----Use Bullets (default)-----")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate;
    private float fireCountDown = 0;

    [Header("-----Use Laser-----")]
    public bool useLaser = false; // If checked to true in inspector, bullet prebab should be left empty

    [SerializeField] Transform laserBarrel;

    [SerializeField] float damageOverTime;
    [SerializeField] float slowAmount;


    [SerializeField] LineRenderer lineRenderer;
    // Need particle system
    // Need Light

    [Header("-----Enemy-----")]
    [SerializeField] Transform target;
    [SerializeField] string enemyTag = "Enemy"; // Turrets only respond to this tag as of now

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void Update()
    {
        if (target == null)
        {
            if (useLaser)
            {               
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                }
            }
            return;
        }

        LockOnTarget();

        if (useLaser)
        {
            Laser();
        }
        else
        {
            if (fireCountDown <= 0)
            {
                Shoot();
                fireCountDown = 1 / fireRate;
            }

            fireCountDown -= Time.deltaTime;
        }
    }
    void LockOnTarget()
    {
        // This gets the distance and direction of the target to face
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(pivot.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        pivot.rotation = Quaternion.Euler(0f, rotation.y, 0f);
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
            // targetEnemy = nearestEnemy.GetComponent<Enemy>
            sphereRange.SetActive(true);
        }
        else
        {
            target = null;
            sphereRange.SetActive(false);
        }
    }
    void Laser()
    {
        // Replace Enemy with the name of script that holds enemy health
        //targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        //targetEnemy.Slow(slowAmount);

        laserBarrel.Rotate(new Vector3(0, 0, 1 * 5));

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
        }

        // Setting the start and end points of the lineRenderer
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
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

    // Visually show max range in scene view 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pivot.position, range);
    }
}

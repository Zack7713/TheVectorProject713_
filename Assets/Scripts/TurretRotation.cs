using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotation : MonoBehaviour
{
    [Header("-----General-----")]
    [SerializeField] float range;
    //[SerializeField] GameObject sphereRange;
    //private Enemy targetEnemy;
    [SerializeField] Material idle;
    [SerializeField] Material live;
    [SerializeField] GameObject status;

    [Header("-----Rotation-----")]
    [SerializeField] float turnSpeed;
    [SerializeField] Transform pivot;

    [Header("-----Use Bullets (default)-----")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform[] shootPositions;
    [SerializeField] float fireRate;

    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip turretShoot;

    private int currentFirePoint = 0; // Turret barrel 1
    private float fireCountDown = 0;

    [Header("-----Recoil Parameters-----")]
    [SerializeField] bool useRecoil;
    [SerializeField] float recoilDistance = 0.1f;
    [SerializeField] float recoilSpeed = 0.2f;

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
            //sphereRange.SetActive(true);
            status.GetComponent<Renderer>().material = live;
        }
        else
        {
            target = null;
            //sphereRange.SetActive(false);
            //commented out to get functionality
            status.GetComponent<Renderer>().material = idle;
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
        //lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
    }

    void Shoot()
    {
        // Get the current firePoint
        Transform shootPos = shootPositions[currentFirePoint];

        // Instantiate the bullet object and get the script from the bullet object
        GameObject turretBullet = (GameObject)Instantiate(bulletPrefab, shootPos.position, shootPos.rotation);
        TurretBullet bullet = turretBullet.GetComponent<TurretBullet>();

        // If we made a bullet, call the bullets function
        if (bullet != null)
        {
            bullet.Seek(target);
        }

        if (useRecoil)
        {
            //StartCoroutine(Recoil(firePoint));
        }

        // Update the current firePoint index for the next shot
        currentFirePoint = (currentFirePoint + 1) % shootPositions.Length;

        // Sound and particles
        audioSource.PlayOneShot(turretShoot);
        
        muzzleFlash.transform.position = shootPos.position;
        muzzleFlash.transform.rotation = shootPos.rotation;
        muzzleFlash.Play();
    }
    
    // Visually show max range in scene view 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pivot.position, range);
    }

    //IEnumerator Recoil(Transform barrel)
    //{
    //    Vector3 originalPosition = barrel.position;

    //    // Move the barrel back
    //    barrel.localPosition -= barrel.forward * recoilDistance;

    //    // Smoothly move the barrel back to its original position
    //    while (Vector3.Distance(barrel.localPosition, originalPosition) > 0.01f)
    //    {
    //        barrel.localPosition = Vector3.Lerp(barrel.localPosition, originalPosition, recoilSpeed * Time.deltaTime);
    //        yield return null;
    //    }

    //    // Ensure the barrel is exactly in its original position
    //    barrel.localPosition = originalPosition;
    //}
}

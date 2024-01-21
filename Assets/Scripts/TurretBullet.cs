using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    private Transform target; // What we're shooting at

    [SerializeField] float speed;

     [SerializeField] int bulletDamage = 1;

    [SerializeField] float explosionRadius = 0;

    [SerializeField] GameObject impactEffect;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // When a turret Instantiates a bullet, it will never miss (the bullets track the target)
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget()
    {
        // Initializing the particle effect
        GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectInstance, 2f);

        if (explosionRadius > 0)
        {
            Explode();
        }
        else
        {
            IDamage dmg = target.GetComponent<IDamage>();

            if (target.transform != transform && dmg != null)
            {
                dmg.takeDamage(bulletDamage);
            } //Damage the enemy here
        }

        Destroy(gameObject); // Destroying the bullet prefab
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                IDamage dmg = target.GetComponent<IDamage>();

                if (target.transform != transform && dmg != null)
                {
                    dmg.takeDamage(bulletDamage);
                }   // Apply Damage to (collider.transform)
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

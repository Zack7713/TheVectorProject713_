using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieMissile : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damageAmount;
    [SerializeField] int destroyTime;
    [SerializeField] int speed;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        //added a check to compare the tag to avoid the Zombies to damage themselves 
        if (!other.CompareTag("Enemy"))
        {
            IDamage dmg = other.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(damageAmount);
            }
        }
        Destroy(gameObject);
    }
}

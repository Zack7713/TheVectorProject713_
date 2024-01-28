using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class claws : MonoBehaviour
{
    [SerializeField] int damageAmount;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickup : MonoBehaviour
{
    [SerializeField] gunStats gun;
    bool triggerSet;
    // Start is called before the first frame update
    void Start()
    {
        gun.ammoCur = gun.ammoMax;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") & !triggerSet)
        {
            triggerSet = true;
            gameManager.instance.playerScript.getGunStats(gun);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnDoor : MonoBehaviour
{
    //make a spawner that works based on time for level
    public Transform spawnObj;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator spawnTimer()
    {
        yield return new WaitForSeconds(timer);
        Instantiate(spawnObj, transform.position, transform.rotation);
        StartCoroutine(spawnTimer());
    }
}

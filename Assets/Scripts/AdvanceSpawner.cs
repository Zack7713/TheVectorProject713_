using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceSpawner : MonoBehaviour
{
    [SerializeField] int difficulty;
    [SerializeField] GameObject objectTOSpawn;
    public int numToSpawn;
    [SerializeField] int timeBetweenSpawns;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] List<GameObject> spawnList = new List<GameObject>();
    public int spawnCount;
    bool isSpawning;
    bool startSpawning;
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && spawnCount < numToSpawn && !isSpawning )
        {
      
            


                //start spawning
                StartCoroutine(spawn());
            
        }
    }
    IEnumerator spawn()
    {
        isSpawning = true;
        int arrayPos = Random.Range(0, spawnPos.Length);
        GameObject objectClone = Instantiate(objectTOSpawn, spawnPos[arrayPos].transform.position, spawnPos[arrayPos].transform.rotation);

     
        spawnList.Add(objectClone);
        spawnCount++;

        yield return new WaitForSeconds(timeBetweenSpawns);
        isSpawning = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = false;
            for (int i = 0; i < spawnList.Count; i++)
            {
                Destroy(spawnList[i]);
            }
            spawnList.Clear();
            spawnCount = 0;
        }
    }
}

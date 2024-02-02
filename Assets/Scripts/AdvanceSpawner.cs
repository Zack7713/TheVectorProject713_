using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AdvanceSpawner : MonoBehaviour
{
    [SerializeField] int difficulty;
    [SerializeField] GameObject[] objectTOSpawn;
    int numToSpawn = 6;
    [SerializeField] int timeBetweenSpawns;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] List<GameObject> spawnList = new List<GameObject>();
    public int spawnCount;
    bool isSpawning;
    bool startSpawning;
    int spawnedCounter;
    int numberToStart = 6; 
    bool hasStarted = false; 
    gameManager manager;
    void Start()
    {

            gameManager.instance.updateGameGoal(gameManager.instance.numToSpawn);


    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && spawnCount < numToSpawn && !isSpawning && spawnCount < 15 && gameManager.instance.wantsToBeginRound == true)
        {

            StartCoroutine(spawn());
            gameManager.instance.hasStartedWaves = true;
        }
        if(numToSpawn == 0 && gameManager.instance.wantsToBeginRound == false&& gameManager.instance.hasStartedWaves == true)
        {
            StopCoroutine(spawn());
            gameManager.instance.updateWaveNumber(+1);
            gameManager.instance.ended = false;
            spawnList.Clear();
            numToSpawn = gameManager.instance.updateSpawnCount(numToSpawn);   
        }

    }
    public void heyIDied()
    {
        numToSpawn--;
        spawnCount--;
    }
    IEnumerator spawn()
    {
        isSpawning = true;
        int arrayPos = Random.Range(0, spawnPos.Length);
        GameObject objectClone = Instantiate(objectTOSpawn[Random.Range(0,objectTOSpawn.Length)], spawnPos[arrayPos].transform.position, spawnPos[arrayPos].transform.rotation);

        objectClone.GetComponent<zombieAI>().mySpawner = this;//change the component from spawnerAI to zombieAI because the zombie AI will be use for most enemy types
        spawnList.Add(objectClone);
        spawnCount++;

        yield return new WaitForSeconds(timeBetweenSpawns);
        isSpawning = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.instance.wantsToBeginRound == true )
        {
            startSpawning = true;
        }
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        startSpawning = false;
    //        for (int i = 0; i < spawnList.Count; i++)
    //        {
    //            Destroy(spawnList[i]);
    //        }
    //        spawnList.Clear();
    //        spawnCount = 0;
    //    }
    //}
}

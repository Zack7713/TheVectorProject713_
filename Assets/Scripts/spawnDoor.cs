using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnDoor : MonoBehaviour
{
    //make a spawner that works based on time for level. no need for now for the difficulty variable

    //[SerializeField] int difficulty;
    [SerializeField] GameObject[] objectTOSpawn;
    public int numToSpawn;
    [SerializeField] int timeBetweenSpawns;
    [SerializeField] GameObject[] spawnPos;
    [SerializeField] List<GameObject> spawnList = new List<GameObject>();
    int spawnCount;
    bool isSpawning;
    bool startSpawning;
    int deadZombies;
    int originalNumToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        //start spawning without the need of a collider, initiate the original num to spawn variable equal to the initial num to spawn, and set the start spawning bool as true
        gameManager.instance.updateGameGoal(numToSpawn);
        originalNumToSpawn = numToSpawn;
        spawnCount = 0;
        startSpawning = true;
        StartCoroutine(spawnRunner());
    }

    // Update is called once per frame
    void Update()
    {
        //check if it started spawning and the spawncount is less than the num to spawn, if true then checking if our spawner is currently spawning, if false the start the coroutine
        if (startSpawning && spawnCount < numToSpawn && spawnCount < originalNumToSpawn)
        {
            if (!isSpawning)
            {
                //start spawning
                StartCoroutine(spawnRunner());
            }

        }
        //else we check if the original num to spawn equals the number of zombies killed or if the spawn count and the current num to spawn are equal. If true, set the start spawning to false, go through the lenght of the array of the spawn position objects, and delete them form each value. 
        //after testing unity wont allow the transform object to be destroy, instead it recommends to destroy the game object instead
        else if (deadZombies == originalNumToSpawn || spawnCount == numToSpawn)
        {
            startSpawning = false;
            if(spawnPos != null)
            {
                for (int i = 0; i < spawnPos.Length; i++)
                {
                    Destroy(spawnPos[i]);
                }
            }
        }
    }
    //function called on the enemy AI taking damage function. is enemy HP reaches 0, decrement the num to spawn, spawn count and increment the num of zombies killed
    public void zombiesKilled()
    {
        numToSpawn--;
        spawnCount--;
        deadZombies++;
    }
    //our spawner enumarator function. When called it turns the is Spawning bool to true, make a random range to select which spawn position will the enemy come spawn from, instantiate an object clone of the enemy spawned, then add that clone to a list, increment the spawn count, return a time between each spawn and turn the is spawning bool back to false at the end
    IEnumerator spawnRunner()
    {
        isSpawning = true;
        yield return new WaitForSeconds(timeBetweenSpawns);
        int arrayPos = Random.Range(0, spawnPos.Length);
        GameObject objectClone = Instantiate(objectTOSpawn[Random.Range(0, objectTOSpawn.Length)], spawnPos[arrayPos].transform.position, spawnPos[arrayPos].transform.rotation);
        objectClone.GetComponent<zombieAI>().myRunner = this;
        spawnList.Add(objectClone);
        spawnCount++;
        isSpawning = false;
    }
}

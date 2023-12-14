using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    [Header("------Menu Components-----")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuUtil;

    public Image playerHPBar;

    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text killCountText;
    [SerializeField] TMP_Text pointAmountText;
    [SerializeField] TMP_Text WaveNumberText;
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;
    public GameObject walkerSpawnPos1;
    public GameObject runnerSpawnPos;
    public GameObject playerDamageScreen;
    public barricadeUnit barricade;
    [Header("------Enemy components-----")]
    [SerializeField] GameObject walkerZombie;
    [SerializeField] GameObject runnerZombie;
    [SerializeField] GameObject creeperZombie;
    [SerializeField] GameObject bossZombie;
    [SerializeField] float playerDistanceWanted;
    float playerDistance;
    public bool isPaused;
    public bool inMenu;
    bool needsToSpawnWalker;
    bool needsToSpawnRunner;
    public float spawnRate;
    public AdvanceSpawner advanceSpawner;
    float timeScaleOriginal;
    int enemiesRemaining;
    int enemiesKilled;
    int pointAmount;
    int waveNumber = 1;
    int waveLimit = 5;


    void Awake()
    {
        updatePointCount(+500);
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
        walkerSpawnPos1 = GameObject.FindWithTag("Walker Spawn 1");
        runnerSpawnPos = GameObject.FindWithTag("Runner Spawn");
        timeScaleOriginal = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {



        if (Input.GetButtonDown("Interact") && menuActive == null)
        {
            utilityMenu();
            menuActive = menuUtil;
            menuActive.SetActive(menuUtil);

        }

        if (Input.GetButtonDown("Cancel") && menuActive == null)
        {
            statePaused();
            menuActive = menuPause;
            menuActive.SetActive(true);
        }


    }

    public void utilityMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void createBarricade()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;

        // Raycast from the center of the screen to determine the position and rotation
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        RaycastHit hit;
        if (pointAmount >= 500)
        {
            updatePointCount(-500);
            if (Physics.Raycast(ray, out hit))
            {

                Bounds barricadeBounds = barricade.GetComponent<Renderer>().bounds;


                Quaternion rotation = Quaternion.LookRotation(transform.forward, hit.normal);


                Vector3 spawnPosition = hit.point + hit.normal * (barricadeBounds.extents.y - 0.01f);


                Instantiate(barricade, spawnPosition, rotation);
            }
            else
            {

                Vector3 spawnPosition = ray.origin + ray.direction * 5f;
                spawnPosition.y += 5f;
                Instantiate(barricade, spawnPosition, transform.rotation);
            }
        }
    }

    public void statePaused()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpaused()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOriginal;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void updateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        enemyCountText.text = enemiesRemaining.ToString("0");

        if (enemiesRemaining <= 0)
        {
            {
                StartCoroutine(youWin());
            }
        }
    }
    IEnumerator youWin()
    {
        //you win
        yield return new WaitForSeconds(3);
        statePaused();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }
    public void updateKillCount(int amount)
    {

        enemiesKilled += amount;
        killCountText.text = enemiesKilled.ToString("0");
        //round increase 

        //if (enemiesKilled >= advanceSpawner.numToSpawn)
        //{

        //    updateWaveNumber(+1);
        //    advanceSpawner.numToSpawn = advanceSpawner.numToSpawn += waveNumber + 6;

        //    if (advanceSpawner.numToSpawn > 250)
        //    {
        //        advanceSpawner.numToSpawn = 250;
        //    }
        //}

        //if(enemiesKilled >= advanceSpawner.numToSpawn)
        //{

        //    updateWaveNumber(+1);
        //    advanceSpawner.numToSpawn = advanceSpawner.numToSpawn += waveNumber+6;

        //    if(advanceSpawner.numToSpawn > 250)
        //    {
        //        advanceSpawner.numToSpawn = 250;
        //    }
        //}
    }


    public void updatePointCount(int amount)
    {
        pointAmount += amount;
        pointAmountText.text = pointAmount.ToString("0000");
    }
    public void updateWaveNumber(int amount)
    {
        waveNumber += amount;
        WaveNumberText.text = waveNumber.ToString("0");
    }
    public void youLose()
    {
        statePaused();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    //IEnumerator spawnRunnerEnemies()
    //{
    //    playerDistance = Vector3.Distance(player.transform.position, runnerSpawnPos.transform.position);
    //    //will need to adjust the player distance but i have the spawner working!!!!!!!!!!!
    //    if (enemiesRemaining <= 20 && playerDistance < playerDistanceWanted)
    //    {

    //        needsToSpawnRunner = true;

    //        Instantiate(runnerZombie, runnerSpawnPos.transform.position, transform.rotation);
    //        yield return new WaitForSeconds(spawnRate);
    //        needsToSpawnRunner = false;

    //    }
    //    else
    //    {
    //        needsToSpawnRunner = false;
    //    }

    //}
    //IEnumerator spawnWalkerEnemies()
    //{
    //    playerDistance = Vector3.Distance(player.transform.position, walkerSpawnPos1.transform.position);
    //    //will need to adjust the player distance but i have the spawner working!!!!!!!!!!!
    //    if (enemiesRemaining <= 20 && playerDistance < playerDistanceWanted)
    //    {

    //        needsToSpawnWalker = true;

    //        Instantiate(walkerZombie, walkerSpawnPos1.transform.position, transform.rotation);
    //        yield return new WaitForSeconds(spawnRate);
    //        needsToSpawnWalker = false;

    //    }
    //    else
    //    {
    //        needsToSpawnWalker = false;
    //    }

    //}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    [Header("------Menu Components-----")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    public Image playerHPBar;

    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text killCountText;
    [SerializeField] TMP_Text pointAmountText;
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;
    public GameObject walkerSpawnPos1;
    [Header("------Enemy components-----")]
    [SerializeField] GameObject walkerZombie;
    [SerializeField] GameObject creeperZombie;
    [SerializeField] GameObject bossZombie;
    public bool isPaused;
    bool needsToSpawn;
    public float spawnRate;

    float timeScaleOriginal;
    int enemiesRemaining;
    int enemiesKilled;
    int pointAmount;

    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
        walkerSpawnPos1 = GameObject.FindWithTag("Walker Spawn 1");
        timeScaleOriginal = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {

        if (!needsToSpawn)
        {
            StartCoroutine(spawnEnemies());
        }




        if (Input.GetButtonDown("Cancel") && menuActive == null)
        {
            statePaused();
            menuActive = menuPause;
            menuActive.SetActive(isPaused);
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


        //if (enemiesRemaining <= 0)//||zombies reach 30)
        //{           
        //    statePaused();
        //    menuActive = menuWin;
        //    menuActive.SetActive(true);
        //}
    }
    public void updateKillCount(int amount)
    {
        enemiesKilled += amount;
        killCountText.text = enemiesKilled.ToString("0");
    }
    public void updatePointCount(int amount)
    {
        pointAmount += amount;
        pointAmountText.text = pointAmount.ToString("0");
    }
    public void youLose()
    {
        statePaused();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    IEnumerator spawnEnemies()
    {
        float playerDistance = Vector3.Distance(player.transform.position, walkerSpawnPos1.transform.position);
        //will need to adjust the player distance but i have the spawner working!!!!!!!!!!!
        if (enemiesRemaining <= 20 && playerDistance < 10)
        {

            needsToSpawn = true;

            Instantiate(walkerZombie, walkerSpawnPos1.transform.position, transform.rotation);
            yield return new WaitForSeconds(spawnRate);
            needsToSpawn = false;

        }
        else
        {
            needsToSpawn = false;
        }

    }
}


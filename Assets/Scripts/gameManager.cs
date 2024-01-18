using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    [Header("------Menu Components-----")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuUtil;
    [SerializeField] GameObject menuInteract;
    [SerializeField] GameObject menuLevels;
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


    float playerDistance;
    public bool isPaused;
    public bool inMenu;
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
        timeScaleOriginal = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {



        if (Input.GetButtonDown("Utility") && menuActive == null)
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
        if (Input.GetButtonDown("Levels") && menuActive == null)
        {
            statePaused();
            menuActive = menuLevels;
            menuActive.SetActive(menuLevels);
        }


    }
    public void interactionMenu()
    {
        menuActive = menuInteract;
        menuActive.SetActive(true);
    }
    public void closeInteractionMenu()
    {
        menuActive.SetActive(false);
        menuActive = null;
    }
    public void utilityMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void closeUtilityMenu()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
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
                // Use the player's forward direction to determine the rotation
                Quaternion rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(player.transform.forward, Vector3.up), Vector3.up);

                // Calculate spawn position based on hit point and normal
                Vector3 spawnPosition = hit.point + hit.normal * barricade.GetComponent<Renderer>().bounds.extents.y;

                Instantiate(barricade, spawnPosition, rotation);
            }
            else
            {
                // If no hit, spawn the barrier in a default direction 
                Vector3 spawnPosition = ray.origin + ray.direction * 5f;
                spawnPosition.y += 5f;

                Instantiate(barricade, spawnPosition, player.transform.rotation);
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
                updateWaveNumber(+1);
                advanceSpawner.numToSpawn = advanceSpawner.numToSpawn += waveNumber + 6;
                updateGameGoal(advanceSpawner.numToSpawn);
                if (advanceSpawner.numToSpawn > 250)
                {
                    advanceSpawner.numToSpawn = 250;
                }
                //  StartCoroutine(youWin());
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


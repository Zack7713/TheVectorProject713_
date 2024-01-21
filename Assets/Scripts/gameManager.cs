using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] GameObject menuHubInteract;
    [SerializeField] GameObject menuLevels;
    [SerializeField] GameObject menuShopKeep;
    [SerializeField] GameObject menuPlayerInventory;
    [SerializeField] GameObject menuBuy;
    public Image playerHPBar;

    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] gunStats pistol;
    [SerializeField] gunStats rifle;
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

    public GameObject barricadePrefab; // Prefab of the barricade
    public GameObject barricadePreviewPrefab; // Prefab for the preview
    public float barricadePreviewHeight = 1f; // Height offset for the preview
    private GameObject barricadePreview; // Instance of the preview

    public GameObject turretPrefab; // Prefab of the barricade
    public GameObject turretPreviewPrefab; // Prefab for the preview
    public float turretPreviewHeight = 1f; // Height offset for the preview
    private GameObject turretPreview; // Instance of the preview

    public bool inBarricadePlacementMode = false;
    public bool inTurretPlacementMode = false;

    float playerDistance;
    public bool isPaused;
    public bool inMenu;
    public float spawnRate;
    //changed advance spawner to spawner door for testing purposes
    public AdvanceSpawner advanceSpawner;
    //public spawnDoor advanceSpawner;
    float timeScaleOriginal;
    int enemiesRemaining;
    int enemiesKilled;
    int pointAmount;
    int waveNumber = 1;
    int waveLimit = 5;



    void Awake()
    {
        updatePointCount(+5000);
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
        if (Input.GetButtonDown("Interact") && menuActive == menuInteract)
        {
            menuActive = null;
            openShopMenu();
        }
        if (Input.GetButtonDown("Interact") && menuActive == menuHubInteract)
        {
            menuActive = null;
            openLevelMenu();
        }
        if (inBarricadePlacementMode)
        {
            UpdateBarricadePreview();

            // Check for confirmation or cancel input (e.g., buttons or keys)
            if (Input.GetMouseButtonDown(0)) // Left mouse button for confirmation
            {
                ConfirmBarricadePlacement();
            }
            else if (Input.GetMouseButtonDown(1)) // Right mouse button for cancellation
            {
                CancelBarricadePlacement();
            }
        }
        if (inTurretPlacementMode)
        {
            UpdateTurretPreview();

            // Check for confirmation or cancel input (e.g., buttons or keys)
            if (Input.GetMouseButtonDown(0)) // Left mouse button for confirmation
            {
                ConfirmTurretPlacement();
            }
            else if (Input.GetMouseButtonDown(1)) // Right mouse button for cancellation
            {
                CancelTurretPlacement();
            }
        }
        playerScript.getGunList(gunList);
    }
    public void interactionMenu()
    {
        menuActive = menuInteract;
        menuActive.SetActive(true);

    }
    public void interactionHubMenu()
    {
        menuActive = menuHubInteract;
        menuActive.SetActive(true);

    }
    public void closeInteractionMenu()
    {
        if(menuActive != null) 
        {
          menuActive.SetActive(false);
          menuActive = null;
        }
       
    }
    public void closeInteractionHubMenu()
    {
        if (menuActive != null)
        {
            menuActive.SetActive(false);
            menuActive = null;
        }

    }
    public void openShopMenu()
    {
      
        statePaused();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        menuInteract.SetActive(false);
        menuActive = menuShopKeep;
        menuActive.SetActive(true);
    }
    public void openLevelMenu()
    {
   
        statePaused();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        menuHubInteract.SetActive(false);
        menuActive = menuLevels;
        menuActive.SetActive(true);
    }
    public void openSellMenu()
    {
        menuShopKeep.SetActive(false);
        menuActive = menuPlayerInventory;
        menuActive.SetActive(true);
    }
    public void sellGunOne()
    {
        if(gunList.Count >=1)
        {
            gunList.RemoveAt(0);
            updatePointCount(+250);
        }
        if(gunList.Count == 0)
        {
           playerScript.showBoughtGun();
        }
        closeMenu();
    }
    public void sellGunTwo()
    {
        if (gunList.Count >= 2)
        {
            gunList.RemoveAt(1);
            updatePointCount(+250);
        }
        playerScript.sellSecondGun();
        closeMenu();
    }
    public void sellGunThree()
    {
        if (gunList.Count >= 3)
        {
            gunList.RemoveAt(2);
            updatePointCount(+250);
        }
        playerScript.sellThirdGun();
        closeMenu();
    }
    public void openBuyMenu()
    {
        menuShopKeep.SetActive(false);
        menuActive = menuBuy;
        menuActive.SetActive(true); 
    }
    public void buyPistol()
    {
        if (pointAmount >= 500)
        {
            if(gunList.Count <= 2)
            {
                gunList.Add(pistol);
                playerScript.showBoughtGun();
                updatePointCount(-500);
            }
         
         
            closeMenu();

      
        }
    }
    public void buyRifle()
    {
        if (pointAmount >= 500)
        {
            if (gunList.Count <= 2)
            {
                gunList.Add(rifle);
                playerScript.showBoughtGun();
                updatePointCount(-500);
            }


            closeMenu();


        }
    }
    public void closeMenu()
    {
        if(isPaused == true)
        {
            stateUnpaused();
        }
        else
        {
            menuActive = null;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

    }


    public void closeUtilityMenu()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (inBarricadePlacementMode)
        {
            DestroyBarricadePreview();
            inBarricadePlacementMode = false;
            inTurretPlacementMode = false;
        }
        // Other closeUtilityMenu logic...
        menuActive.SetActive(false);
        menuActive = null;
    }
    public void utilityMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        inBarricadePlacementMode = true;
       
    }
    public void CreateBarricadePreview()
    {
        menuActive = null;
        menuUtil.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        barricadePreview = Instantiate(barricadePreviewPrefab);
    }
    public void CreateTurretPreview()
    {
        menuActive = null;
        menuUtil.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        turretPreview = Instantiate(turretPreviewPrefab);
    }
    public void DestroyBarricadePreview()
    {
        if (barricadePreview != null)
        {
            Destroy(barricadePreview);
        }
    }
    public void DestroyTurretPreview()
    {
        if (turretPreview != null)
        {
            Destroy(turretPreview);
        }
    }
    private void UpdateBarricadePreview()
    {
        if (barricadePreview != null)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 spawnPosition = hit.point + hit.normal * barricadePreviewHeight;
                barricadePreview.transform.position = spawnPosition;

                Quaternion rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(player.transform.forward, Vector3.up), Vector3.up);
                barricadePreview.transform.rotation = rotation;
            }
            else
            {
                Vector3 spawnPosition = ray.origin + ray.direction * 5f;
                spawnPosition.y += barricadePreviewHeight;
                barricadePreview.transform.position = spawnPosition;

                barricadePreview.transform.rotation = player.transform.rotation;
            }
        }
    }
    private void UpdateTurretPreview()
    {
        if (turretPreview != null)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 spawnPosition = hit.point + hit.normal * barricadePreviewHeight;
                turretPreview.transform.position = spawnPosition;

                Quaternion rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(player.transform.forward, Vector3.up), Vector3.up);
                turretPreview.transform.rotation = rotation;
            }
            else
            {
                Vector3 spawnPosition = ray.origin + ray.direction * 5f;
                spawnPosition.y += barricadePreviewHeight;
                turretPreview.transform.position = spawnPosition;

                turretPreview.transform.rotation = player.transform.rotation;
            }
        }
    }
    private void ConfirmBarricadePlacement()
    {
        if (barricadePreview != null)
        {
            // Perform the actual instantiation of the barricade
            Instantiate(barricadePrefab, barricadePreview.transform.position, barricadePreview.transform.rotation);
            DestroyBarricadePreview();
            inBarricadePlacementMode = false;
        }
    }
    private void ConfirmTurretPlacement()
    {
        if (turretPreview != null)
        {
            // Perform the actual instantiation of the barricade
            Instantiate(turretPrefab, turretPreview.transform.position, turretPreview.transform.rotation);
            DestroyTurretPreview();
            inTurretPlacementMode = false;
        }
    }
    private void CancelBarricadePlacement()
    {
        DestroyBarricadePreview();
        inBarricadePlacementMode = false;
    }
    private void CancelTurretPlacement()
    {
        DestroyTurretPreview();
        inTurretPlacementMode = false;
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
                //StartCoroutine(youWin());
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


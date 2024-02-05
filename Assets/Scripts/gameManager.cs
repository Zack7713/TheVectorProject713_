using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    [Header("------Menu Components-----")]
    public GameObject menuActive;
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
    [SerializeField] GameObject menuUpgrade;
    [SerializeField] GameObject menuTowerSell;
    [SerializeField] GameObject menuRoundStart;
    [SerializeField] GameObject menuOptions;
    public Image playerHPBar;

    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    private List<GameObject> builtTowers = new List<GameObject>();
    public gunStats currentGun;
    public gunStats pistol;
    [SerializeField] gunStats rifle;
    [SerializeField] gunStats shotgun;
    public bool hasPistol = false;
    public bool hasRifle = false;
    public bool hasShotgun = false;
    public TMP_Text enemyCountText;
    [SerializeField] TMP_Text killCountText;
    public TMP_Text pointAmountText;
    public TMP_Text WaveNumberText;
    public TMP_Text BuildUnitText;
    public TMP_Text gunNameText;
    public TMP_Text gunAmmoCur;
    public TMP_Text gunAmmoMax;
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;
    public GameObject walkerSpawnPos1;
    public GameObject runnerSpawnPos;
    public GameObject playerDamageScreen;
    public barricadeUnit barricade;

    public GameObject barricadePrefab; // Prefab of the barricade
    public GameObject barricadePreviewPrefab; // Prefab for the preview
    public float barricadePreviewHeight = 0f; // Height offset for the preview
    private GameObject barricadePreview; // Instance of the preview

    public GameObject turretPrefab; // Prefab of the barricade
    public GameObject turretPreviewPrefab; // Prefab for the preview
    public float turretPreviewHeight = 0f; // Height offset for the preview
    private GameObject turretPreview; // Instance of the preview

    public GameObject turretStandardPrefab; // Prefab of the barricade
    public GameObject turretStandardPreviewPrefab; // Prefab for the preview
    public float turretStandardPreviewHeight = 0f; // Height offset for the preview
    private GameObject turretStandardPreview; // Instance of the preview

    public bool inBarricadePlacementMode = false;
    public bool inTurretPlacementMode = false;
    public bool inTurretStandardPlacementMode = false;
    public bool inTowerBarricadeSellMode = false;

    float playerDistance;
    public bool isPaused;
    public bool inMenu;
    public float spawnRate;
    public bool wantsToBeginRound = false;
    public bool hasStartedWaves = false;
    //changed advance spawner to spawner door for testing purposes
    public AdvanceSpawner advanceSpawner;
    //public spawnDoor advanceSpawner;
    float timeScaleOriginal;
    public int buildUnits;
    [SerializeField] int buildUnitLimit = 35;
    public int enemiesRemaining;
    int enemiesKilled;
    public int pointAmount;
    public int waveNumber = 1;
    int waveLimit = 5;
    public int numToSpawn = 6;
    int result =6;
    public bool ended = false;

    void Awake()
    {

        updatePointCount(+10000);

        instance = this;
        player = GameObject.FindWithTag("Player");

        playerScript = player.GetComponent<playerController>();
        
    
        //playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
        timeScaleOriginal = Time.timeScale;
        // advanceSpawner.wantsToBeginRound = false;
        if(wantsToBeginRound == false)
        {
            playerScript.HPOrig = 10; 
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (!wantsToBeginRound)
        {
            enemiesRemaining = 0;
            enemyCountText.text = enemiesRemaining.ToString();

        }
        if (Input.GetButtonDown("Utility") && menuActive == null)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            statePaused();
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
        if (Input.GetButtonDown("Interact") && menuActive == menuRoundStart && wantsToBeginRound == false)
        {
            int currentNumToSpawn = numToSpawn;
            getEnemyCount();
            wantsToBeginRound = true;
            if (enemiesRemaining == 0)
            {
                enemiesRemaining = result;
            }
            enemyCountText.text = enemiesRemaining.ToString();
            hasStartedWaves = true;
            closeInteractionMenu();

        }
        if (hasStartedWaves ==false)
        {
            numToSpawn = 6;
        }
        if (hasStartedWaves == true && enemiesRemaining == 0)
        {
            wantsToBeginRound = false;
        }
        if (inBarricadePlacementMode)
        {
            UpdateBarricadePreview();



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

            if (Input.GetMouseButtonDown(0)) // Left mouse button for confirmation
            {
                ConfirmTurretPlacement();
            }
            else if (Input.GetMouseButtonDown(1)) // Right mouse button for cancellation
            {
                CancelTurretPlacement();
            }
        }
        if (inTurretStandardPlacementMode)
        {
            UpdateStandardTurretPreview();


            if (Input.GetMouseButtonDown(0)) // Left mouse button for confirmation
            {
                ConfirmStandardTurretPlacement();
            }
            else if (Input.GetMouseButtonDown(1)) // Right mouse button for cancellation
            {
                CancelStandardTurretPlacement();
            }
        }
        if (inTowerBarricadeSellMode)
        {


            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("ray");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {

                    if (hit.collider.CompareTag("Barricade"))
                    {
                        SellBarricadeTower(hit.collider.gameObject);
                        inTowerBarricadeSellMode = false;
                    }
                    if (hit.collider.CompareTag("MissileTurret"))
                    {
                        SellMissileTurret(hit.collider.gameObject);
                        inTowerBarricadeSellMode = false;
                    }
                    if (hit.collider.CompareTag("BasicTurret"))
                    {
                        SellBasicTurret(hit.collider.gameObject);
                        inTowerBarricadeSellMode = false;
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                inTowerBarricadeSellMode = false;
                if (menuActive != null)
                {
                    menuActive.SetActive(false);
                    menuActive = null;
                }
            }
        }
        playerScript.getGunList(gunList);
        if(currentGun != null )
        {
            if (currentGun != null && currentGun.gunName != gunNameText.text)
            {
                gunNameText.text = currentGun.gunName;
                gunAmmoCur.text = currentGun.ammoCur.ToString();
                gunAmmoMax.text = currentGun.ammoMax.ToString();
            }
            if(currentGun.ammoCur != currentGun.ammoMax)
            {
                gunAmmoCur.text = currentGun.ammoCur.ToString();
                gunAmmoMax.text = currentGun.ammoMax.ToString();
            }

        }

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
    public void RoundStartPrompt()
    {
        if (wantsToBeginRound == false)
        {
            menuActive = menuRoundStart;
            menuActive.SetActive(true);
        }

    }
    public void closeInteractionMenu()
    {
        if (menuActive != null)
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

        statePaused();//pausing the game 
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;//locking the cursor 
        menuInteract.SetActive(false);//closing the previous menu if needed
        menuActive = menuShopKeep;//setting the active 
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
    public void openUpgradeMenu()
    {
        menuShopKeep.SetActive(false);
        menuActive = menuUpgrade;
        menuActive.SetActive(true);
    }
    public void OpenTowerSellMenu()
    {

        if (buildUnits > 0)
        {
            menuUtil.SetActive(false);
            menuActive = menuTowerSell;
            menuActive.SetActive(true);
            inTowerBarricadeSellMode = true;
            stateUnpaused();
        }

    }
    public void upgradeGunOne()
    {
        if (pointAmount >= gunList[0].upgradeCost)
        {
            if (gunList[0].isUpgraded == true)
            {


                gunList[0].upgradeCost *= 2;

            }
            updatePointCount(-gunList[0].upgradeCost);
            increaseGunDamage(0, 1);
            gunList[0].isUpgraded = true;
            closeShopMenu();
        }
    }
    public void upgradeGunTwo()
    {
        if (pointAmount >= gunList[1].upgradeCost)
        {
            if (gunList[1].isUpgraded == true)
            {


                gunList[1].upgradeCost *= 2;

            }
            updatePointCount(-gunList[1].upgradeCost);
            increaseGunDamage(1, 1);
            updatePointCount(-500);
            closeShopMenu();
        }
    }
    public void upgradeGunThree()
    {
        if (pointAmount >= gunList[2].upgradeCost)
        {
            if (gunList[2].isUpgraded == true)
            {


                gunList[2].upgradeCost *= 2;

            }
            updatePointCount(-gunList[2].upgradeCost);
            increaseGunDamage(2, 1);
            updatePointCount(-500);
            closeShopMenu();
        }
    }
    public void increaseGunDamage(int gunIndex, int amount)
    {
        playerScript.getGunList(gunList);
        if (gunIndex >= -1 && gunIndex < gunList.Count)
        {
            gunList[gunIndex].shootDamage += amount;
            playerScript.shootDamage = gunList[gunIndex].shootDamage;
        }

    }
    public void sellGun(int gunIndex)
    {
        if (gunList.Count > 0)
        {
            if (gunIndex >= 0 && gunIndex < gunList.Count)
            {
                gunStats gunToSell = gunList[gunIndex];
                if (gunToSell != null && gunToSell.gunName == ("Pistol"))
                {
                    gunToSell.ResetPistol();
                    hasPistol = false;
                }
                else if (gunToSell != null && gunToSell.gunName == ("Rifle"))
                {
                    gunToSell.ResetRifle();
                    hasRifle = false;
                }
                else if (gunToSell != null && gunToSell.gunName == ("Shotgun"))
                {
                    gunToSell.ResetShotgun();
                    hasShotgun = false;
                }

                gunList.RemoveAt(gunIndex);

            }

        }

    }
    public void sellGunOne()
    {
        if (gunList.Count >= 1)
        {
            sellGun(0);
            updatePointCount(+250);
        }
        if (gunList.Count == 0)
        {
            playerScript.showBoughtGun();
        }
        closeShopMenu();
    }
    public void sellGunTwo()
    {
        if (gunList.Count >= 2)
        {
            sellGun(1);
            updatePointCount(+250);
        }
        if (gunList.Count == 0)
        {
            playerScript.showBoughtGun();
        }

        playerScript.sellSecondGun();
        closeShopMenu();
    }
    public void sellGunThree()
    {
        if (gunList.Count >= 3)
        {
            sellGun(2);
            updatePointCount(+250);
        }
        if (gunList.Count == 0)
        {
            playerScript.showBoughtGun();
        }

        playerScript.sellThirdGun();
        closeShopMenu();
    }
    public void openBuyMenu()
    {
        menuShopKeep.SetActive(false);
        menuActive = menuBuy;
        menuActive.SetActive(true);
    }
    public void buyPistol()
    {
        if (pointAmount >= 500 && !hasPistol)
        {
            if (gunList.Count < 3)
            {
                gunStats newGun = Instantiate(pistol);
                newGun.Initialize();
                gunList.Add(newGun);
                playerScript.showBoughtGun();
                updatePointCount(-500);
                hasPistol = true;
            }


            closeShopMenu();


        }
    }
    public void buyRifle()
    {
        if (pointAmount >= 500 && !hasRifle)
        {
            if (gunList.Count < 3)
            {
                
                gunStats newGun = Instantiate(rifle);
                newGun.Initialize();
                gunList.Add(newGun);
                playerScript.showBoughtGun();
                updatePointCount(-500);
                hasRifle = true;
            }


            closeShopMenu();


        }
    }
    public void buyShotgun()
    {
        if (pointAmount >= 500 && !hasShotgun)
        {
            if (gunList.Count < 3)
            {
                gunStats newGun = Instantiate(shotgun);
                newGun.Initialize();
                gunList.Add(newGun);
                playerScript.showBoughtGun();
                updatePointCount(-500);
                hasShotgun = true;

            }
            closeShopMenu();

        }
    }
    public void closeMenu()
    {
        if (isPaused == true)
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
    public void closeShopMenu()
    {
        if (isPaused == true)
        {
            stateUnpaused();
        }


        menuActive = menuInteract;
        menuActive.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


    }
    public void closeUtilityMenu()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (inBarricadePlacementMode)
        {
            DestroyBarricadePreview();
            inBarricadePlacementMode = false;

        }
        else if (inTurretPlacementMode)
        {
            DestroyTurretPreview();
            inTurretPlacementMode = false;
        }
        else if (inTurretStandardPlacementMode)
        {
            DestroyTurretPreview();
            inTurretStandardPlacementMode = false;
        }
        stateUnpaused();
        if (menuActive == true)
        {
            menuActive.SetActive(false);
            menuActive = null;
        }

    }
    public void utilityMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        //inBarricadePlacementMode = true;


    }
    void SellBarricadeTower(GameObject tower)
    {

        buildUnits--;
        BuildUnitText.text = buildUnits.ToString("0");

        updatePointCount(+100);

        Destroy(tower);
        if (menuActive != null)
        {
            menuActive.SetActive(false);
            menuActive = null;
        }
    }
    void SellMissileTurret(GameObject tower)
    {


        buildUnits = buildUnits - 3;
        BuildUnitText.text = buildUnits.ToString("0");

        updatePointCount(+250);

        Destroy(tower);
        if (menuActive != null)
        {
            menuActive.SetActive(false);
            menuActive = null;
        }
    }
    void SellBasicTurret(GameObject tower)
    {

        buildUnits = buildUnits - 2;
        BuildUnitText.text = buildUnits.ToString("0");
        updatePointCount(+150);
        Destroy(tower);
        if (menuActive != null)
        {
            menuActive.SetActive(false);
            menuActive = null;
        }
    }
    public void CreateBarricadePreview()
    {
        if (pointAmount >= 200)
        {


            menuActive.SetActive(false);
            menuActive = null;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            barricadePreview = Instantiate(barricadePreviewPrefab);
            stateUnpaused();
        }
        else
            closeUtilityMenu();
    }
    public void CreateTurretPreview()
    {
        if (pointAmount >= 500)
        {


            menuActive.SetActive(false);
            menuActive = null;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            turretPreview = Instantiate(turretPreviewPrefab);
            stateUnpaused();
        }
        else
            closeUtilityMenu();
    }
    public void CreateStandardTurretPreview()
    {
        if (pointAmount >= 300)
        {
            if (menuActive != null)
            {
                menuActive.SetActive(false);
                menuActive = null;
            }
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            turretStandardPreview = Instantiate(turretStandardPreviewPrefab);
            stateUnpaused();
        }
        else
            closeUtilityMenu();
    }
    public void DestroyBarricadePreview()
    {
        if (barricadePreview != null)
        {
            Destroy(barricadePreview);
            inBarricadePlacementMode = false;
        }
    }
    public void DestroyTurretPreview()
    {
        if (turretPreview != null)
        {
            Destroy(turretPreview);
            inTurretPlacementMode = false;
        }
    }
    public void DestroyStandardTurretPreview()
    {
        if (turretStandardPreview != null)
        {
            Destroy(turretStandardPreview);
            inTurretStandardPlacementMode = false;
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
                // Use hit.normal for the upward direction
                Vector3 spawnPosition = hit.point + hit.normal * barricadePreviewHeight;
                barricadePreview.transform.position = spawnPosition;

                Quaternion rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(player.transform.forward, hit.normal), hit.normal);
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
                Vector3 spawnPosition = hit.point + hit.normal * turretPreviewHeight;

                turretPreview.transform.position = spawnPosition;


                Quaternion rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(player.transform.forward, Vector3.up), Vector3.up);
                turretPreview.transform.rotation = rotation;
            }
            else
            {
                Vector3 spawnPosition = ray.origin + ray.direction * 5f;
                spawnPosition.y += turretPreviewHeight;
                turretPreview.transform.position = spawnPosition;

                turretPreview.transform.rotation = player.transform.rotation;
            }
        }
    }
    private void UpdateStandardTurretPreview()
    {

        if (turretStandardPreview != null)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 spawnPosition = hit.point + hit.normal * turretStandardPreviewHeight;

                turretStandardPreview.transform.position = spawnPosition;


                Quaternion rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(player.transform.forward, Vector3.up), Vector3.up);
                turretStandardPreview.transform.rotation = rotation;
            }
            else
            {
                Vector3 spawnPosition = ray.origin + ray.direction * 5f;
                spawnPosition.y += turretStandardPreviewHeight;
                turretStandardPreview.transform.position = spawnPosition;

                turretStandardPreview.transform.rotation = player.transform.rotation;
            }
        }
    }
    private void ConfirmBarricadePlacement()
    {
        buildUnits++;
        if (buildUnits > buildUnitLimit)
        {
            DestroyBarricadePreview();
            buildUnits--;
            return;
        }
        buildUnits--;
        if (barricadePreview != null)
        {
            Vector3 spawnPosition = barricadePreview.transform.position;
            if (IsPositionOnNavMesh(spawnPosition))
            {
                Instantiate(barricadePrefab, spawnPosition, barricadePreview.transform.rotation);
                DestroyBarricadePreview();
                inBarricadePlacementMode = false;
                buildUnits++;
                BuildUnitText.text = buildUnits.ToString("0");
                updatePointCount(-200);
            }
            else
            {
                DestroyBarricadePreview();
                // Optionally, provide feedback to the player that the placement is invalid.
            }
        }

    }
    private void ConfirmTurretPlacement()
    {
        buildUnits = buildUnits + 3;
        if (buildUnits > buildUnitLimit)
        {
            DestroyTurretPreview();
            buildUnits = buildUnits - 3;
            return;
        }
        buildUnits = buildUnits - 3;
        if (turretPreview != null)
        {
            if (!IsPositionOnNavMesh(turretPreview.transform.position))
            {
                DestroyTurretPreview();

                return;
            }

            Instantiate(turretPrefab, turretPreview.transform.position, turretPreview.transform.rotation);
            DestroyTurretPreview();
            inTurretPlacementMode = false;
            buildUnits = buildUnits + 3;
            BuildUnitText.text = buildUnits.ToString("0");
            updatePointCount(-500);
        }
    }
    private void ConfirmStandardTurretPlacement()
    {
        buildUnits = buildUnits + 2;
        if (buildUnits > buildUnitLimit)
        {
            DestroyStandardTurretPreview();
            buildUnits = buildUnits - 2;
            return;
        }
        buildUnits = buildUnits - 2;
        if (turretStandardPreview != null)
        {
            if (!IsPositionOnNavMesh(turretStandardPreview.transform.position))
            {
                DestroyStandardTurretPreview();
                return;
            }

            Instantiate(turretStandardPrefab, turretStandardPreview.transform.position, turretStandardPreview.transform.rotation);
            DestroyStandardTurretPreview();
            inTurretStandardPlacementMode = false;
            buildUnits = buildUnits + 2;
            BuildUnitText.text = buildUnits.ToString("0");
            updatePointCount(-300);
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
    private void CancelStandardTurretPlacement()
    {
        DestroyTurretPreview();
        inTurretStandardPlacementMode = false;
    }
    private bool IsPositionOnNavMesh(Vector3 position)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(position, out hit, 1f, NavMesh.AllAreas);
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
        if (menuActive == true && inTowerBarricadeSellMode == false)
        {
            menuActive.SetActive(false);
            menuActive = null;
        }

    }
    public int updateSpawnCount(int spawnCount)
    {
        getEnemyCount();
        spawnCount = numToSpawn;
        return spawnCount;
    }
    public void getEnemyCount()
    {

        if (enemiesRemaining <= 0 && hasStartedWaves == true && ended == false)
        {



            numToSpawn = numToSpawn + waveNumber + 6;
            result = numToSpawn;
            ended = true;
         

            //wantsToBeginRound = false;
            //do something else 


            if (waveNumber == 10)
            {
                StartCoroutine(youWin());
            }


        }
        else if(hasStartedWaves == false)
        {
            numToSpawn = 6;
        }
        numToSpawn = result;
    }
    public void updateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        
        enemyCountText.text = enemiesRemaining.ToString("0");


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

    public void optionsMenuOpen()
    {
        menuActive = menuOptions;
        menuActive.SetActive(true);

    }
    public void optionsMenuClose()
    {
        menuActive = menuOptions;
        menuActive.SetActive(false);

    }
}











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
    [SerializeField] GameObject menuUpgrade;
    [SerializeField] GameObject menuTowerSell;
    [SerializeField] GameObject menuRoundStart;
    public Image playerHPBar;

    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    private List<GameObject> builtTowers = new List<GameObject>();
    [SerializeField] gunStats pistol;
    [SerializeField] gunStats rifle;
    [SerializeField] gunStats shotgun;
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text killCountText;
    [SerializeField] TMP_Text pointAmountText;
    [SerializeField] TMP_Text WaveNumberText;
    [SerializeField] TMP_Text BuildUnitText;
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
    //changed advance spawner to spawner door for testing purposes
    public AdvanceSpawner advanceSpawner;
    //public spawnDoor advanceSpawner;
    float timeScaleOriginal;
    public int buildUnits;
    [SerializeField] int buildUnitLimit = 35;
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
       // advanceSpawner.wantsToBeginRound = false;
    }
    // Update is called once per frame
    void Update()
    {



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
        if (Input.GetButtonDown("Interact") && menuActive == menuRoundStart && advanceSpawner.wantsToBeginRound == false)
        {
           
            advanceSpawner.wantsToBeginRound = true;
            closeInteractionMenu();
        
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
        if (advanceSpawner.wantsToBeginRound == false)
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
        increaseGunDamage(0, 1);
        updatePointCount(-500);
        closeMenu();
    }
    public void upgradeGunTwo()
    {
        increaseGunDamage(1, 1);
        updatePointCount(-500);
        closeMenu();
    }
    public void upgradeGunThree()
    {
        increaseGunDamage(2, 1);
        updatePointCount(-500);
        closeMenu();
    }
    public void increaseGunDamage(int gunIndex, int amount)
    {
        playerScript.getGunList(gunList);
        if (gunIndex >= 0 && gunIndex < gunList.Count)
        {
            gunList[gunIndex].shootDamage += amount;

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
                }
                else if (gunToSell != null && gunToSell.gunName == ("Rifle"))
                {
                    gunToSell.ResetRifle();
                }
                else if (gunToSell != null && gunToSell.gunName == ("Shotgun"))
                {
                    gunToSell.ResetShotgun();
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
        closeMenu();
    }
    public void sellGunTwo()
    {
        if (gunList.Count >= 2)
        {
            sellGun(1);
            updatePointCount(+250);
        }
        playerScript.sellSecondGun();
        closeMenu();
    }
    public void sellGunThree()
    {
        if (gunList.Count >= 3)
        {
            sellGun(2);
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
            if (gunList.Count < 3)
            {
                gunStats newGun = Instantiate(pistol);
                newGun.Initialize();
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
            if (gunList.Count < 3)
            {
                gunList.Add(rifle);
                playerScript.showBoughtGun();
                updatePointCount(-500);
            }


            closeMenu();


        }
    }
    public void buyShotgun()
    {
        if (pointAmount >= 500)
        {
            if (gunList.Count < 3)
            {
                gunList.Add(shotgun);
                playerScript.showBoughtGun();
                updatePointCount(-500);
            }


            closeMenu();


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
        inBarricadePlacementMode = true;


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
        menuActive = null;
        menuUtil.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        barricadePreview = Instantiate(barricadePreviewPrefab);
        stateUnpaused();
    }
    public void CreateTurretPreview()
    {
        menuActive = null;
        menuUtil.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        turretPreview = Instantiate(turretPreviewPrefab);
        stateUnpaused();
    }
    public void CreateStandardTurretPreview()
    {
        menuActive = null;
        menuUtil.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        turretStandardPreview = Instantiate(turretStandardPreviewPrefab);
        stateUnpaused();
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
    public void DestroyStandardTurretPreview()
    {
        if (turretStandardPreview != null)
        {
            Destroy(turretStandardPreview);
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
        if (buildUnits + 1 > buildUnitLimit)
        {
            DestroyBarricadePreview();
            return;
        }

        if (barricadePreview != null)
        {
            Vector3 spawnPosition = barricadePreview.transform.position;
            if (IsPositionOnNavMesh(spawnPosition))
            {
                Instantiate(barricadePrefab, spawnPosition, barricadePreview.transform.rotation);
                DestroyBarricadePreview();
                inBarricadePlacementMode = false;
                buildUnits += 1;
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

        if (buildUnits + 3 > buildUnitLimit)
        {
            DestroyTurretPreview();
            return;
        }
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

        if (buildUnits + 2 > buildUnitLimit)
        {
            DestroyStandardTurretPreview();
            return;
        }

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
                if (waveNumber == 10)
                {
                    StartCoroutine(youWin());
                }

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
}




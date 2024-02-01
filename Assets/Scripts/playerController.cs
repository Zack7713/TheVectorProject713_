using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components-----")]
    [SerializeField] private CharacterController controller;
    [SerializeField] AudioSource aud;
    [SerializeField] Animator animPlayer;
    [Header("----- Stats -----")]
    [Range(1, 10)][SerializeField] int HP;
    [Range(1, 8)][SerializeField] private float playerSpeed;
    [Range(8, 30)][SerializeField] private float jumpHeight;
    [Range(-10, -40)][SerializeField] private float gravityValue;
    [Range(1, 3)][SerializeField] int jumpMax;
    [Range(1.5f, 3)][SerializeField] float sprintMod;
    [Range(.5f, 1)][SerializeField] float crouchMod;
    [Range(.25f, .75f)][SerializeField] float proneMod;
    [SerializeField] List<inventoryItems> inventoryList = new List<inventoryItems>();
    [Header("----- Weapon -----")]
    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] GameObject gunModel;
    [SerializeField] float knifeColSpeed;
    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] soundHurt;
    [Range(0, 1)][SerializeField] float soundHurtVol;
    [SerializeField] AudioClip[] soundSteps;
    [Range(0, 1)][SerializeField] float soundStepVol;
    PlayerSpawnPos spawnPos;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Vector3 move;
    private int jumpCount;
    private bool isShooting;
    int HPOrig;
    int selectedGun;
    bool isPlayingSteps;
    bool isSprinting;
    bool isInteracting;
    bool isKnifing;
    float animSpeed;
    private bool isCrouching;
    private float originalPlayerSpeed;
    private bool isProne;

    AdvanceSpawner advanceSpawner;
    private gameManager gameManagerInstance;
    private float currentRecoilAngle;
    private bool isRecoiling;

    private void Start()
    {
        originalPlayerSpeed = playerSpeed;
        HPOrig = HP;
        respawnPlayer();
        animPlayer = GetComponent<Animator>();

        gameManager.instance.playerScript.respawnPlayer();

        gameManagerInstance = gameManager.instance;
    }

    void Update()
    {



        if (Input.GetButtonDown("MeleeAttack"))
        {
            StartCoroutine(pAttack());
        }


        if (!gameManager.instance.isPaused)
            Camera.main.transform.Rotate(-currentRecoilAngle, 0f, 0f);
        {
            HandleRecoil();
            CameraRecoil();

            if (gunList.Count > 0)
            {
                if (Input.GetButton("Shoot") && !isShooting)
                    StartCoroutine(shoot());

                selectGun();
            }

            movement();
        }
    }

    public void TeleportPlayer(Vector3 position)
    {
        controller.enabled = false;
        transform.position = position;
        controller.enabled = true;
    }


    void HandleRecoil()
    {
        if (!isRecoiling && currentRecoilAngle > 0f)
        {
            float recoveryStep = gunList[selectedGun].recoilRecoverySpeed * Time.deltaTime;
            currentRecoilAngle = Mathf.Lerp(currentRecoilAngle, 0f, recoveryStep);
            currentRecoilAngle = Mathf.Clamp(currentRecoilAngle, 0f, gunList[selectedGun].maxRecoilAngle);
        }

    }
    void CameraRecoil()
    {
        if (!isRecoiling && currentRecoilAngle > 0f)
        {
            // Calculate the recoil rotation
            Quaternion recoilRotation = Quaternion.Euler(-currentRecoilAngle, 0f, 0f);

            // Apply the recoil rotation smoothly
            Camera.main.transform.localRotation = Quaternion.Slerp(Camera.main.transform.localRotation, recoilRotation, Time.deltaTime * gunList[selectedGun].recoilRecoverySpeed);

            // Reset the recoil angle if it reaches zero
            if (Mathf.Approximately(Camera.main.transform.localRotation.eulerAngles.x, 0f))
            {
                currentRecoilAngle = 0f;
                isRecoiling = false;
            }
        }
    }

        IEnumerator playSteps()
    {
        isPlayingSteps = true;
        aud.PlayOneShot(soundSteps[Random.Range(0, soundSteps.Length - 1)], soundStepVol);
        if (!isSprinting)
        {
            yield return new WaitForSeconds(0.5f);
        }
        else
            yield return new WaitForSeconds(0.3f);

        isPlayingSteps = false;
    }

    public void respawnPlayer()
    {
        HP = HPOrig;
        updatePlayerUI();
        if (spawnPos != null)
        {
            controller.enabled = false;
            transform.position = gameManager.instance.playerSpawnPos.transform.position;
            gameManager.instance.buildUnits = 0;
            gameManager.instance.wantsToBeginRound = false;
            controller.enabled = true;
        }

    }
    PlayerSpawnPos position; 
    public void respawnPlayerOnLoad(Vector3 SpawnPosit)
    {
        HP = HPOrig;
        updatePlayerUI();

        controller.enabled = false;
        transform.position = SpawnPosit;
        gameManager.instance.buildUnits = 0;
    
        controller.enabled = true;


    }
    void movement()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
        //prone();
        crouch();
        sprint();



        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && move.normalized.magnitude > 0.3f && !isPlayingSteps)
        {
            StartCoroutine(playSteps());
        }
        {

        }
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            jumpCount = 0;
        }



        move = Input.GetAxis("Horizontal") * transform.right +
               Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move.magnitude >= 0.1f)
        {
            // Set the Animator parameter "Speed" based on your movement speed
            animPlayer.SetFloat("Speed", playerSpeed);
        }
        else
        {
            // If there's no movement, set the "Speed" parameter to 0
            animPlayer.SetFloat("Speed", 0f);
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVelocity.y = jumpHeight;
            jumpCount++;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    IEnumerator shoot()
    {
        if (gunList[selectedGun].ammoCur > 0)
        {
            gunList[selectedGun].ammoCur--; // Subtract pellets from ammo count
            aud.PlayOneShot(gunList[selectedGun].shootSound, gunList[selectedGun].shootSoundVolume);
            isShooting = true;

            // Retrieve shootSpread from gunStats class
            float currentSpread = gunList[selectedGun].shootSpread;

            for (int pellet = 0; pellet < gunList[selectedGun].gunPellets; pellet++)
            {
                // Calculate random spread within the specified radius with a fixed angle
                float spreadAngle = Random.Range(0f, 360f);

                // Convert spread angle to radians
                float spreadRadians = spreadAngle * Mathf.PI / 180f;

                Vector2 randomSpread = new Vector2(Mathf.Cos(spreadRadians), Mathf.Sin(spreadRadians)) * currentSpread;

                // Modify raycast origin with random spread
                Vector3 viewportPoint = new Vector2(0.5f + randomSpread.x, 0.5f + randomSpread.y);
                Ray ray = Camera.main.ViewportPointToRay(viewportPoint);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, shootDist))
                {
                    Instantiate(gunList[selectedGun].hitEffect, hit.point, transform.rotation);
                    IDamage dmg = hit.collider.GetComponent<IDamage>();

                    if (hit.transform != transform && dmg != null)
                    {
                        dmg.takeDamage(shootDamage);
                    }
                }
            }
            float recoilAngle = gunList[selectedGun].recoilAngle;
            float recoilRecoverySpeed = gunList[selectedGun].recoilRecoverySpeed;
            Vector3 cameraForward = Camera.main.transform.forward;



            currentRecoilAngle += recoilAngle;
            isRecoiling = true;


            yield return new WaitForSeconds(shootRate);
            isRecoiling = false;
            isShooting = false;
        }
    }
    IEnumerator pAttack()
    {
        isKnifing = true;
        animPlayer.SetTrigger("MeleeAttack");
        yield return new WaitForSeconds(knifeColSpeed);
        isKnifing = false;
    }
    void crouch()
    {

        if (Input.GetButtonDown("Crouch") && !isSprinting)
        {
            // Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - .5f, Camera.main.transform.position.z);

            controller.height = 1.5f;
            playerSpeed *= crouchMod;
            isCrouching = true;

        }
        else if (Input.GetButtonUp("Crouch") && !isSprinting)
        {
            // Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + .5f, Camera.main.transform.position.z);
            controller.height = 2;
            playerSpeed = originalPlayerSpeed;
            isCrouching = false;
        }
    }
    //void prone()
    //{

    //    if (Input.GetButtonDown("Prone") && !isSprinting && !isCrouching)
    //    {
    //        // Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - .1f, Camera.main.transform.position.z);
    //        controller.height = 1f;
    //        playerSpeed *= proneMod;
    //        isProne = true;
    //    }
    //    else if (Input.GetButtonUp("Prone") && !isSprinting && !isCrouching)
    //    {
    //        // Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + .1f, Camera.main.transform.position.z);
    //        controller.height = 3;
    //        playerSpeed = originalPlayerSpeed;
    //        isProne = false;

    //    }
    //}
    void sprint()
    {

        if (Input.GetButtonDown("Sprint") && !isCrouching && !isProne)
        {
            playerSpeed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint") && !isCrouching && !isProne)
        {
            playerSpeed = originalPlayerSpeed;
            isSprinting = false;
        }

    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        aud.PlayOneShot(soundHurt[Random.Range(0, soundHurt.Length - 1)], soundHurtVol);
        updatePlayerUI();
        StartCoroutine(playerFlashDamage());

        if (HP <= 0)
        {
            //im dead
            gameManager.instance.youLose();
        }
    }
    IEnumerator playerFlashDamage()
    {
        gameManager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamageScreen.SetActive(false);
    }
    IEnumerator interact()
    {
        isInteracting = true;
        yield return new WaitForSeconds(0.5f);
        isInteracting = false;
    }
    public void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }
    public void getGunStats(gunStats gun)
    {
        gunList.Add(gun);
        shootDamage = gun.shootDamage;
        shootDist = gun.shootDist;
        shootRate = gun.shootRate;
        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial;

        selectedGun = gunList.Count - 1;

        //// Assuming PlayerLoader is static
        //if (PlayerLoader.instance != null)
        //{
        //    //call the player loader instance to add the gunstat?
        //    PlayerLoader.instance.acquiredGuns.Add(gun.gunName);
        //    //PlayerLoader.instance.acquiredGuns.Add(gun);
        //}
        //else
        //{
        //    Debug.LogError("PlayerLoader instance not found!");
        //}

    }
    public void getInventoryItem(inventoryItems item)
    {
        inventoryList.Add(item);

    }
    public void showBoughtGun()
    {
        if (gunList.Count > 1)
        {
            selectedGun++;
        }

        changeGun();
    }
    public void getGunList(List<gunStats> guns)
    {
        gunList = guns;
    }
    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            selectedGun++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            changeGun();
        }
    }
    public void sellSecondGun()
    {
        if (gunList.Count >= 1)
        {
            shootDamage = gunList[0].shootDamage;
            shootDist = gunList[0].shootDist;
            shootRate = gunList[0].shootRate;

            gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[0].model.GetComponent<MeshFilter>().sharedMesh;
            gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[0].model.GetComponent<MeshRenderer>().sharedMaterial;
            isShooting = false;
        }
    }
    public void sellThirdGun()
    {
        if (gunList.Count >= 2)
        {
            shootDamage = gunList[1].shootDamage;
            shootDist = gunList[1].shootDist;
            shootRate = gunList[1].shootRate;

            gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[1].model.GetComponent<MeshFilter>().sharedMesh;
            gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[1].model.GetComponent<MeshRenderer>().sharedMaterial;
            isShooting = false;
        }
    }
    public void changeGun()
    {
        if (gunList == null || gunList.Count == 0)
        {
            // Handle the case where gunList is null or empty
            shootDamage = 0;
            shootDist = 0;
            shootRate = 0;
            gunModel.GetComponent<MeshFilter>().sharedMesh = null;
            gunModel.GetComponent<MeshRenderer>().sharedMaterial = null;
            isShooting = false;
            return;
        }

        // Check if selectedGun is within a valid range
        if (selectedGun < 0 || selectedGun >= gunList.Count)
        {
            // Handle the case where selectedGun is out of range
            return;
        }

        // Update properties based on the selected gun
        shootDamage = gunList[selectedGun].shootDamage;
        shootDist = gunList[selectedGun].shootDist;
        shootRate = gunList[selectedGun].shootRate;

        // Update gun model
        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;

        isShooting = false;
    }
}



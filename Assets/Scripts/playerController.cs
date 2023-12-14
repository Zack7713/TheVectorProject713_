using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components-----")]
    [SerializeField] private CharacterController controller;
    [SerializeField] AudioSource aud;
    [Header("----- Stats -----")]
    [Range(1, 10)][SerializeField] int HP;
    [Range(1, 8)][SerializeField] private float playerSpeed;
    [Range(8, 30)][SerializeField] private float jumpHeight;
    [Range(-10, -40)][SerializeField] private float gravityValue;
    [Range(1, 3)][SerializeField] int jumpMax;
    [Range(1.5f, 3)][SerializeField] float sprintMod;

    [Header("----- Weapon -----")]
    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] GameObject gunModel;
    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] soundHurt;
    [Range(0, 1)][SerializeField] float soundHurtVol;
    [SerializeField] AudioClip[] soundSteps;
    [Range(0, 1)][SerializeField] float soundStepVol;
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

    private void Start()
    {

        HPOrig = HP;
        respawnPlayer();

    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        {


            if (gunList.Count > 0)
            {
                if (Input.GetButton("Shoot") && !isShooting)
                    StartCoroutine(shoot());

                selectGun();
            }
            // if(Input.GetButton("Interact"))
            movement();
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

        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;


    }

    void movement()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

        sprint();

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && move.normalized.magnitude > 0.3f && !isPlayingSteps)
        {
            //StartCoroutine(playSteps());
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


            gunList[selectedGun].ammoCur--;
            //aud.PlayOneShot(gunList[selectedGun].shootSound, gunList[selectedGun].shootSoundVol);
            isShooting = true;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {
                Instantiate(gunList[selectedGun].hitEffect, hit.point, transform.rotation);
                IDamage dmg = hit.collider.GetComponent<IDamage>();

                if (hit.transform != transform && dmg != null)
                {
                    dmg.takeDamage(shootDamage);
                }
            }

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            playerSpeed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            playerSpeed /= sprintMod;
            isSprinting = false;
        }
    }
    public void takeDamage(int amount)
    {
        HP -= amount;
       // aud.PlayOneShot(soundHurt[Random.Range(0, soundHurt.Length - 1)], soundHurtVol);
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
    void changeGun()
    {
        shootDamage = gunList[selectedGun].shootDamage;
        shootDist = gunList[selectedGun].shootDist;
        shootRate = gunList[selectedGun].shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
        isShooting = false;
    }
}

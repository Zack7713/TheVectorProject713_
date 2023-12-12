using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components-----")]
    [SerializeField] private CharacterController controller;
    [SerializeField] Animator anim;

    [Header("----- Stats -----")]
    [Range(1, 10)][SerializeField] int HP;
    [Range(1, 8)][SerializeField] private float playerSpeed;
    [Range(1, 30)][SerializeField] private float jumpHeight;
    [Range(-10, -40)][SerializeField] private float gravityValue;
    [Range(1, 3)][SerializeField] int jumpMax;
    [Range(1.5f, 3)][SerializeField] float sprintMod;

    [Header("----- Weapon -----")]
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] GameObject gunModel;


    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Vector3 move;
    private int jumpCount;
    private bool isShooting;
    int HPOrig;

    private void Start()
    {

        HPOrig = HP;
        respawnPlayer();
     
    }

    void Update()
    {
   
        movement();
        
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
        if (!gameManager.instance.isPaused && Input.GetButton("Shoot") && !isShooting)
            StartCoroutine(shoot());
        groundedPlayer = controller.isGrounded;
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
        anim.SetFloat("Speed", playerVelocity.normalized.magnitude);
    }
    IEnumerator shoot()
    {

        isShooting = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            playerSpeed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            playerSpeed /= sprintMod;
        }
    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();
        StartCoroutine(playerFlashDamage());
        if (HP <= 0)
        {
            //im dead
            gameManager.instance.youLose();
        }
    }
    public void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }
    IEnumerator playerFlashDamage()
    {
        gameManager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamageScreen.SetActive(false);
    }
}
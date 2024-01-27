using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using UnityEngine.AI;

//COMMENTED OUT TEST LOGIC FOR IS WALKER ENEMY!!!

public class zombieAI : MonoBehaviour, IDamage
{
    [Header("----- Components-----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] Animator anim;
    [SerializeField] Collider[] clawsCol;
    [SerializeField] Collider damageCol;

    [Header("----- Enemy Stats-----")]
    [SerializeField] int HP;
    [SerializeField] int viewCone;
    [SerializeField] int targetFaceSpeed;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;
    [SerializeField] int zombiePointValue;
    [SerializeField] float animSpeedTrans;
    [SerializeField] bool isRunner;
    [SerializeField] bool isStalker;
    [SerializeField] bool isJumper;
    [SerializeField] bool isWalker;
    [SerializeField] bool isBloater;

    [Header("----- Attacks -----")]
    [SerializeField] float attackRate;
    [SerializeField] float attackDur;
    [SerializeField] GameObject infectedMissile;
    [SerializeField] Transform attackPos;


    //roam variables
    Vector3 playerDir;
    Vector3 survDir;
    Vector3 startingPos;
    bool destinationChosen;
    //attacking
    bool isAttacking;
    //player is in range
    bool playerInRange;
    //survivor is in range
    bool survivorInRange;
    //bool isAlive;
    float angleToPlayer;
    //float angleToSurv;
    float stoppingDistOrig;
    float distanceToPlayer;
    //float distanceToSurv;
    public AdvanceSpawner mySpawner;
    public spawnDoor myRunner;//test code to use the updated spawner door
    //private List<GameObject> survivorNPC;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager.instance.updateGameGoal(1); //our game objctive is been updated in the game manager with the advance spawnerds
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        //survivorNPC = new List<GameObject>(GameObject.FindGameObjectsWithTag("Survivor"));
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            float animSpeed = agent.velocity.normalized.magnitude;
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));
            //adding the bool check for other types of zombies
            if (isRunner)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
            }
            //if (isWalker)
            //{
            //    if (survivorInRange && !canSeePlayer())
            //    {
            //        StartCoroutine(roam());
            //    }
            //    else if (!survivorInRange)
            //    {
            //        StartCoroutine(roam());
            //    }
            //}
            else
            {
                if (playerInRange && !canSeePlayer())
                {
                    StartCoroutine(roam());
                }
                else if (!playerInRange)
                {
                    StartCoroutine(roam());
                }
            }
        }
    }
    IEnumerator roam()
    {
        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamPauseTime);

            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
    }
    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        //use the same logic for the survivor so enemies can see them and follow 
        //for (int i = 0; i < survivorNPC.Count; i++)
        //{
        //    survDir = survivorNPC[i].transform.position - headPos.position;
        //    angleToSurv = Vector3.Angle(survDir, transform.forward);
        //}
        Debug.DrawRay(headPos.position, playerDir);
        //Debug.Log(angleToPlayer);
        RaycastHit hit;
        //if (isWalker)
        //{
        //    if (Physics.Raycast(headPos.position, survDir, out hit))
        //    {
        //        if (hit.collider.CompareTag("Survivor") && angleToSurv <= viewCone)
        //        {
        //            for (int j = 0; j < survivorNPC.Count; j++)
        //            {
        //                distanceToSurv = Vector3.Distance(transform.position, survivorNPC[j].transform.position);
        //                isAlive = survivorNPC[j].GetComponent<npcAI>().isActiveAndEnabled;
        //                //by getting the component of the npcAI to check if the agent is active and enable and adding a while loop to check if the target is still active, we keep the enemy attacking only one target at a time when there are more survivors in the scene. 
        //                while (isAlive)
        //                {
        //                    agent.SetDestination(survivorNPC[j].transform.position);
        //                    if (agent.remainingDistance < agent.stoppingDistance)
        //                    {
        //                        faceTarget();
        //                    }
        //                    if (distanceToSurv <= agent.stoppingDistance && !isAttacking)
        //                    {
        //                        //still need to add more logic
        //                        StartCoroutine(attack());
        //                    }
        //                }
        //                survivorInRange = false;
        //            }
        //            agent.stoppingDistance = stoppingDistOrig;
        //        }
        //    }
        //    return true;
        //}
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);
                //adding another check in the can see player for the different zombie types, since the following code affects the destination and movement of the zombie when getting the player enters their sphere collider
                if (isStalker)
                {
                    if (!isAttacking)
                    {
                        StartCoroutine(shootMissiles(attackDur, attackRate));
                    }
                    if (agent.remainingDistance < agent.stoppingDistance)
                    {
                        faceTarget();
                    }
                    agent.stoppingDistance = stoppingDistOrig;
                    return true;
                }
                if (isRunner)
                {
                    agent.SetDestination(gameManager.instance.player.transform.position);
                    if (!isAttacking)
                    {
                        StartCoroutine(attack());
                    }
                    if (agent.remainingDistance < agent.stoppingDistance)
                    {
                        faceTarget();
                    }
                    agent.stoppingDistance = stoppingDistOrig;
                    return true;
                }
                if (distanceToPlayer <= 3f)
                {
                    StartCoroutine(attack());
                }
                else
                {
                    agent.SetDestination(gameManager.instance.player.transform.position);
                    if (!isAttacking)
                    {
                        StartCoroutine(attack());
                    }
                    if (agent.remainingDistance < agent.stoppingDistance)
                    {
                        faceTarget();
                    }
                    agent.stoppingDistance = stoppingDistOrig;
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }
    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * targetFaceSpeed);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (isWalker)
        {
            if (other.CompareTag("Survivor"))
            {
                survivorInRange = true;
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }
    IEnumerator attack()
    {
        isAttacking = true;
        anim.SetTrigger("Slash");
        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }
    IEnumerator shootMissiles(float duration, float rate)
    {
        isAttacking = true;
        anim.SetTrigger("Shoot");
        float remainingTime = 0;
        while (remainingTime < duration)
        {
            createMissile();
            remainingTime += rate;
            yield return new WaitForSeconds(rate);
        }
        isAttacking = false;
    }
    public void createMissile()
    {
        Instantiate(infectedMissile, attackPos.position, transform.rotation);
    }
    public void clawsColOn()
    {
        for (int i = 0; i < clawsCol.Length; i++)
        {
            if (clawsCol[i] != null)
            {
                clawsCol[i].enabled = true;
            }
        }
    }
    public void clawsColOff()
    {
        for (int i = 0; i < clawsCol.Length; i++)
        {
            if (clawsCol[i] != null)
            {
                clawsCol[i].enabled = false;
            }
        }
    }
    public void takeDamage(int amount)
    {
        HP -= amount;

        StopAllCoroutines();//used stop all coroutines

        //checking if enemie is a certain type of zombie so it wont break its logic
        if (HP <= 0)
        {
            //comment out mySpawner and add myRunner for test purposes
            anim.SetBool("Dead", true);
            agent.enabled = false;
            damageCol.enabled = false;
            mySpawner.heyIDied();
            //myRunner.zombiesKilled();
            gameManager.instance.updateKillCount(+1);
            gameManager.instance.updateGameGoal(-1);
            //model.sharedMaterial.color = Color.white;
            gameManager.instance.updatePointCount(+zombiePointValue);
            anim.SetBool("Dead", true);
            agent.enabled = false;
            damageCol.enabled = false;
            //Destroy(gameObject);//take out the destroy object so we can play our dead animation
        }
        else
        {
            gameManager.instance.updatePointCount(+10);
            isAttacking = false;
            anim.SetTrigger("Damage");
            destinationChosen = false;
            //StartCoroutine(flashRed());
            if (!isRunner)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
            }
        }
    }
    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;

    }

}

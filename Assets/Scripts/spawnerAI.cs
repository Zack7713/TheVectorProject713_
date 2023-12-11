using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;

public class spawnerAI : MonoBehaviour , IDamage
{
    [Header("----- Components-----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] Animator anim;
    [SerializeField] Collider leftClawCol;
    [SerializeField] Collider rightClawCol;
    [SerializeField] Collider damageCol;

    [Header("----- Enemy Stats-----")]
    [SerializeField] int HP;
    [SerializeField] int viewCone;
    [SerializeField] int targetFaceSpeed;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;
    [SerializeField] float animSpeedTrans;


    [Header("----- Attacks -----")]
    [SerializeField] float attackRate;
    [SerializeField] Transform attackPos;

    //roam variables
    Vector3 playerDir;
    Vector3 startingPos;
    bool destinationChosen;
    //attacking
    bool isAttacking;
    //player is in range
    bool playerInRange;
    float angleToPlayer;
    float stoppingDistOrig;
    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            float animSpeed = agent.velocity.normalized.magnitude;
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));
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
        Debug.DrawRay(headPos.position, playerDir);
        Debug.Log(angleToPlayer);
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
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
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
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
    public void leftColOn()
    {
        leftClawCol.enabled = true;
    }
    public void leftColOff()
    {
        leftClawCol.enabled = false;
    }
    public void rightColOn()
    {
        rightClawCol.enabled = true;
    }
    public void rightColOff()
    {
        rightClawCol.enabled = false;
    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashRed());

        if (HP <= 0)
        {
            Destroy(gameObject);
            gameManager.instance.updateKillCount(+1);
            gameManager.instance.updateGameGoal(-1);
            model.sharedMaterial.color = Color.white;
        }
    }
    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;

    }
}

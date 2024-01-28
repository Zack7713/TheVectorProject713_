using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npcAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform headPos;
    [SerializeField] Animator anim;
    [SerializeField] Collider damageCol;
    [Header("----- NPC Stats -----")]
    [Range(1,10)][SerializeField] int HP;
    [SerializeField] int viewCone;
    [SerializeField] int targetFaceSpeed;
    [SerializeField] float animSpeedTrans;
    bool playerInRange;
    bool isWaving;
    float angleToPlayer;
    float distanceToPlayer;
    Vector3 playerDir;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && canSeePlayer())
        {
            distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);
            callPlayer(true);
            if (distanceToPlayer <= agent.stoppingDistance && isWaving)
            {
                callPlayer(false);
            }
        }
        else if (!playerInRange || !canSeePlayer())
        {
            if (isWaving)
            {
                callPlayer(false);
            }
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
                faceTarget();
                return true;
            }
        }
        return false;
    }
    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * targetFaceSpeed);
    }
    public void callPlayer(bool wave)
    {
        isWaving = wave;
        anim.SetBool("Call", isWaving);
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
        }
    }
    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            agent.enabled = false;
            damageCol.enabled = false;
            Destroy(gameObject);
        }
    }
}

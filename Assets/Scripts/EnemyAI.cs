using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header ("Model")]
    [SerializeField] Renderer model;
    Material enemyMaterial; //Using material so we don't have to use sharedMaterial which affects all objects of the same prefab

    [Header("NavMesh")]
    [SerializeField] NavMeshAgent agent;
    bool playerInRange;

    [Header("Enemy Weapon")]
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] Transform shootPos;
    bool isShooting;


    [Header ("Enemy Stats")]
    [SerializeField] int HP;

    // Start is called before the first frame update
    void Start()
    {
        enemyMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashRed());

        if (HP <= 0)
        {
            Destroy(gameObject);
            model.sharedMaterial.color = Color.white;
        }
    }
    IEnumerator flashRed()
    {
        enemyMaterial.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        enemyMaterial.color = Color.white;

    }

}

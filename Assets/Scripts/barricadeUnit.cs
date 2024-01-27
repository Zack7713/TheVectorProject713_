using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barricadeUnit : MonoBehaviour, IDamage
{

    [SerializeField] int HP;
    [SerializeField] Renderer model;
    bool isAttacking;
    zombieAI zombie;
    playerController controller;
    int attackRate = 3;
    [SerializeField] string enemyTag = "Enemy";


    void Update()
    {
        GameObject zombieObject = GameObject.FindWithTag(enemyTag);
        if (zombieObject != null)
        {
            zombie = zombieObject.GetComponent<zombieAI>();
        }

        //GameObject playerObject = GameObject.FindWithTag("Player");
        //if (playerObject != null)
        //{
        //    controller = playerObject.GetComponent<playerController>();
        //}
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashRed());
        if (HP <= 0)
        {
            Destroy(gameObject);
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            isAttacking = true;

            if (zombie != null)
            {
                StartCoroutine(attack());
               
            }
        }
        //if (other.CompareTag("Player"))
        //{
        //    isAttacking = true;

        //    if (controller != null)
        //    {
        //        StartCoroutine(attack());

        //    }
        //}
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {

            isAttacking = false;
            StopCoroutine(attack());
        }
        //if (other.CompareTag("Player"))
        //{

        //    isAttacking = false;
        //    StopCoroutine(attack());
        //}
    }
    IEnumerator attack()
    {
        {
            while (isAttacking)
            {
                // Deal damage to the player every 3 seconds
                //controller.takeDamage(1);
                zombie.takeDamage(1);
                yield return new WaitForSeconds(attackRate);
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

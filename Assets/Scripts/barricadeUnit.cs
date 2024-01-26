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
    // Start is called before the first frame update
    void Update()
    {
        // Try to find the zombieAI script on an object with the "Zombie" tag
        GameObject zombieObject = GameObject.FindWithTag("Zombie");
        if (zombieObject != null)
        {
            zombie = zombieObject.GetComponent<zombieAI>();
        }

        // Try to find the playerController script on an object with the "Player" tag
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            controller = playerObject.GetComponent<playerController>();
        }
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
        if (other.CompareTag("Zombie"))
        {
            isAttacking = true;

            if (zombie != null)
            {
                StartCoroutine(attack());
               
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {

            isAttacking = false;
            StopCoroutine(attack());
       
       
        
     
        }
    }
    IEnumerator attack()
    {
        {
            while (isAttacking)
            {
                // Deal damage to the player every 3 seconds
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

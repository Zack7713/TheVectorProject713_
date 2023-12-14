using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barricadeUnit : MonoBehaviour, IDamage
{

    [SerializeField] int HP;
    [SerializeField] Renderer model;
    // Start is called before the first frame update

    public void takeDamage(int amount)
    {
        HP -= amount;
        flashRed();
        if (HP <= 0)
        {
            Destroy(gameObject);
        }

    }
    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;

    }
}

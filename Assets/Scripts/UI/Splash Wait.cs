using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashWait : MonoBehaviour
{
    public int splashWait;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForSplash());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator WaitForSplash()
    {
       yield return new WaitForSeconds (splashWait);
        SceneManager.LoadScene(1);

       
    }
}

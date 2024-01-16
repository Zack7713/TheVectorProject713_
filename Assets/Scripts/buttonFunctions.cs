using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gameManager.instance.stateUnpaused();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnpaused();
    }

    public void quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    public void respawnPlayer()
    {
        gameManager.instance.playerScript.respawnPlayer();
        gameManager.instance.stateUnpaused();
    }

    public void spawnBarricade()
    {
        gameManager.instance.createBarricade();
    }

    public void closeUtilityMenu()
    {
        gameManager.instance.closeUtilityMenu();
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Application.Quit();
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

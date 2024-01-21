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
    public void closeMenu()
    {
        gameManager.instance.closeMenu();
    }
    public void respawnPlayer()
    {
        gameManager.instance.playerScript.respawnPlayer();
        gameManager.instance.stateUnpaused();
    }

    public void spawnBarricade()
    {
     
        gameManager.instance.inBarricadePlacementMode = true;
        gameManager.instance.CreateBarricadePreview();
      
    }
    public void spawnturret()
    {
        gameManager.instance.inTurretPlacementMode = true;
        gameManager.instance.CreateTurretPreview();
    }
    public void closeUtilityMenu()
    {
        gameManager.instance.closeUtilityMenu();
        gameManager.instance.DestroyBarricadePreview();
    }
    public void openBuyMenu()
    { 
        gameManager.instance.openBuyMenu();
    }
    public void openSellMenu()
    {
        gameManager.instance.openSellMenu();
    }
    public void sellFirstGun()
    {
        gameManager.instance.sellGunOne();
    }
    public void sellSecondGun()
    {
        gameManager.instance.sellGunTwo();
    }
    public void sellThirdGun()
    {
        gameManager.instance.sellGunThree();
    }
    public void buyPistolInShop()
    {
        gameManager.instance.buyPistol();
    }
    public void buyRifleInShop()
    {
        gameManager.instance.buyRifle();
    }
}


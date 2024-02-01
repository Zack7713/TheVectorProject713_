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
    AdvanceSpawner AdvanceSpawner;
    public void resume()
    {
        gameManager.instance.stateUnpaused();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.buildUnits = 0;
        gameManager.instance.BuildUnitText.text = gameManager.instance.buildUnits.ToString("0");
        gameManager.instance.pointAmount = 5000;
        gameManager.instance.pointAmountText.text = gameManager.instance.pointAmount.ToString("0000");
        gameManager.instance.enemyCountText.text = gameManager.instance.advanceSpawner.spawnCount.ToString();
        
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
    public void spawnTurret()
    {
        gameManager.instance.inTurretPlacementMode = true;
        gameManager.instance.CreateTurretPreview();
    }
    public void spawnStandardTurret()
    {
        gameManager.instance.inTurretStandardPlacementMode = true;
        gameManager.instance.CreateStandardTurretPreview();
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
    public void openUpgradeMenu()
    {
         gameManager.instance.openUpgradeMenu();
    }
    public void SellTowerMode()
    {
        gameManager.instance.OpenTowerSellMenu();
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
    public void upgradeMenuOne()
    {
        gameManager.instance.upgradeGunOne();
    }
    public void upgradeMenuTwo()
    {
        gameManager.instance.upgradeGunTwo();
    }
    public void upgradeMenuThree()
    {
        gameManager.instance.upgradeGunThree();
    }
    public void buyRifleInShop()
    {
        gameManager.instance.buyRifle();
    }
    public void buyShotgunInShop()
    {
        gameManager.instance.buyShotgun();
        
    }
}


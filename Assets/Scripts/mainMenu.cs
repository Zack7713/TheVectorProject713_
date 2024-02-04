using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class mainMenu : MonoBehaviour
{
    public static mainMenu instance;
    public GameObject menuActive;
    [SerializeField] GameObject menuOptions;
    [SerializeField] GameObject menuCredits;


    // Start is called before the first frame update
    void Start()
    {
           instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadGame()
    {
        
        SceneManager.LoadScene(3);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }


    public void OpenSettings()
    {

        menuOptions.SetActive(true);

    }
    public void CloseSettings()
    {

        menuOptions.SetActive(false);

    }
    public void OpenCredits()
    {

        menuCredits.SetActive(true);

    }
    public void CloseCredits()
    {

        menuCredits.SetActive(false);

    }

    public void quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}

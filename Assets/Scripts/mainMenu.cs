using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class mainMenu : MonoBehaviour
{

    public Slider musicVolumeSlider;
    public Slider soundEffectsVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGame()
    {
        
        SceneManager.LoadScene(7);
        SceneManager.LoadScene(6, LoadSceneMode.Additive);
    }
    //public void LoadGameTunnel()
    //{
    //    SceneManager.LoadScene(7);
    //    SceneManager.LoadScene(6, LoadSceneMode.Additive);
    //}
    //public void LoadGameCity()
    //{
    //    SceneManager.LoadScene(7);
    //    SceneManager.LoadScene(6, LoadSceneMode.Additive);
    //}

    public void ApplySettings()
    {
        SetVolume();
    }

    private void SetVolume()
    {
        float musicVolume = musicVolumeSlider.value;
        float soundEffectsVolume = soundEffectsVolumeSlider.value;

        GameObject persistence = GameObject.Find("Persistence");

        if (persistence != null)
        {
            persistence.GetComponent<optionsSettings>().SetMusicVolume(musicVolume);
            persistence.GetComponent<optionsSettings>().SetSoundEffectsVolume(soundEffectsVolume);
        }
        else
        {
            Debug.LogError("Persistence object not found!");
        }

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

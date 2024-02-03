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
    public static mainMenu instance;
    public GameObject menuActive;
    [SerializeField] GameObject menuOptions;

    public Slider musicVolumeSlider;
    public Slider soundEffectsVolumeSlider;

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

    public void OpenSettings()
    {

        menuOptions.SetActive(true);

    }
    public void CloseSettings()
    {

        menuOptions.SetActive(false);

    }
    //public void ApplySettings()
    //{
    //    SetVolume();
    //}
    //private void SetVolume()
    //{
    //    float musicVolume = musicVolumeSlider.value;
    //    float soundEffectsVolume = soundEffectsVolumeSlider.value;

    //    // Apply volume settings to audio sources or any other components requiring volume control
    //    // For example, you can use AudioManager.SetMusicVolume(musicVolume) and AudioManager.SetSoundEffectsVolume(soundEffectsVolume)

    //    // Save the current volume settings
    //    PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    //    PlayerPrefs.SetFloat("SoundEffectsVolume", soundEffectsVolume);
    //    PlayerPrefs.Save();
    //}
    public void quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}

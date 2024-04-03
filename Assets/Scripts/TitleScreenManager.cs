using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.RuntimeSet;

public class TitleScreenManager : MonoBehaviour {
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject howToPlayScreen;

    [SerializeField] private GameObjectRuntimeSetScrub targetsRuntimeSetScrub;

    private void Start() {
        titleScreen.SetActive(true);
        settingsScreen.SetActive(false);
        controlsScreen.SetActive(false);
        howToPlayScreen.SetActive(false);
        
        targetsRuntimeSetScrub.items.Clear();
    }

    public void Quit() {
        Application.Quit();
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeMenuScreen(GameObject screen) {
        titleScreen.SetActive(false);
        settingsScreen.SetActive(false);
        controlsScreen.SetActive(false);
        howToPlayScreen.SetActive(false);
        
        screen.SetActive(true);
    }
}
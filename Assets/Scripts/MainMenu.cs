using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField] private Eflatun.SceneReference.SceneReference gameScene;
    [SerializeField] private Eflatun.SceneReference.SceneReference settingsScene;
    [SerializeField] private Eflatun.SceneReference.SceneReference menuScene;

    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void StartGame() {
        SceneManager.LoadScene(gameScene.Name);
    }
    
    public void SettingsMenu() {
        SceneManager.LoadScene(settingsScene.Name);
    }
    
    public void QuitGame() {
        Application.Quit();
    }
    
    public void BackToMenu() {
        SceneManager.LoadScene(menuScene.Name);
    }
}
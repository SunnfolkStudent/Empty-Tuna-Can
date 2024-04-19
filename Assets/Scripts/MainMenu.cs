using UnityEngine;
using UnityEngine.SceneManagement;
using SceneReference = Eflatun.SceneReference.SceneReference;

public class MainMenu : MonoBehaviour {
    [SerializeField] private SceneReference playerScene;
    [SerializeField] private SceneReference gameScene;
    [SerializeField] private SceneReference settingsScene;
    [SerializeField] private SceneReference menuScene;

    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame() {
        SceneManager.LoadScene(playerScene.Name);
        SceneManager.LoadScene(gameScene.Name, LoadSceneMode.Additive);
        Destroy(gameObject);
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
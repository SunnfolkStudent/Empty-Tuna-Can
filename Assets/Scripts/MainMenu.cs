using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Singleton;
using SceneReference = Eflatun.SceneReference.SceneReference;

public class MainMenu : Singleton<MainMenu> {
    [SerializeField] private SceneReference settingsScene;
    [SerializeField] private SceneReference howToPlayScene;
    [SerializeField] private SceneReference menuScene;
    
    [SerializeField] private SceneReference playerScene;
    
    [SerializeField] private SceneReference storyModeScene;
    [SerializeField] private SceneReference versusModeScene;
    [SerializeField] private SceneReference endlessModeScene;
    
    protected override void Awake() {
        base.Awake();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        DontDestroyOnLoad(gameObject);
    }
    
    public void StartStoryMode() {
        SceneManager.LoadScene(storyModeScene.Name);
        Destroy(gameObject);
    }
    
    public void StartVersusMode() {
        SceneManager.LoadScene(playerScene.Name);
        SceneManager.LoadScene(versusModeScene.Name, LoadSceneMode.Additive);
        Destroy(gameObject);
    }
    
    public void StartEndlessMode() {
        SceneManager.LoadScene(playerScene.Name);
        SceneManager.LoadScene(endlessModeScene.Name, LoadSceneMode.Additive);
        Destroy(gameObject);
    }
    
    public void SettingsMenu() {
        SceneManager.LoadScene(settingsScene.Name);
        Destroy(gameObject);
    }
    
    public void HowToPlayMenu() {
        SceneManager.LoadScene(howToPlayScene.Name);
        Destroy(gameObject);
    }
    
    public void QuitGame() {
        Application.Quit();
    }
    
    public void BackToMenu() {
        SceneManager.LoadScene(menuScene.Name);
        Destroy(gameObject);
    }
}
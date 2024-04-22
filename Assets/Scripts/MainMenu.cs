using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using SceneReference = Eflatun.SceneReference.SceneReference;

public class MainMenu : MonoBehaviour {
    [SerializeField] private SceneReference playerScene;
    [SerializeField] private SceneReference gameScene;
    [SerializeField] private SceneReference settingsScene;
    [SerializeField] private SceneReference menuScene;
    [SerializeField] private SceneReference[] singleAreaScenes;
    [SerializeField] private SceneReference versusModeScene;

    [SerializeField] private GameObject versusModeManager;

    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        DontDestroyOnLoad(gameObject);
    }

    public void StartStoryMode() {
        PlayerManager.FriendlyFire = false;
        //TODO: Start Cutscene
        
        SceneManager.LoadScene(playerScene.Name);
        SceneManager.LoadScene(gameScene.Name, LoadSceneMode.Additive);
        Destroy(gameObject);
    }
    
    public void StartVersusMode() {
        PlayerManager.FriendlyFire = true;
        SceneManager.LoadScene(playerScene.Name);
        SceneManager.LoadScene(singleAreaScenes.GetRandom().Name, LoadSceneMode.Additive);
        SceneManager.LoadScene(versusModeScene.Name, LoadSceneMode.Additive);
        Destroy(gameObject);
    }
    
    public void StartEndlessMode() {
        PlayerManager.FriendlyFire = false;
        
        // SceneManager.LoadScene(playerScene.Name);
        // SceneManager.LoadScene(endlessModeScenes.Name, LoadSceneMode.Additive);
        // Destroy(gameObject);
    }
    
    public void SettingsMenu() {
        SceneManager.LoadScene(settingsScene.Name);
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
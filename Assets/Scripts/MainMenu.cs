using System.Collections;
using ModeManagers;
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

    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip startMode;
    
    private bool hasSelectedOption;
    
    protected override void Awake() {
        base.Awake();
        PauseMenu.UnpauseCondition = () => true;
        StoryModeManager.ExitingToMenu = false;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        DontDestroyOnLoad(gameObject);
    }
    
    public void StartStoryMode() {
        if (hasSelectedOption) return;
        hasSelectedOption = true;
        StartCoroutine(StartStory());
    }

    private IEnumerator StartStory() {
        animator.Play(startMode.name);
        
        yield return new WaitForSeconds(startMode.length);
        
        SceneManager.LoadScene(storyModeScene.Name);
        Destroy(gameObject);
    }
    
    public void StartVersusMode() {
        if (hasSelectedOption) return;
        hasSelectedOption = true;
        StartCoroutine(StartVS());
    }
    
    private IEnumerator StartVS() {
        animator.Play(startMode.name);
        
        yield return new WaitForSeconds(startMode.length);
        
        SceneManager.LoadScene(playerScene.Name);
        SceneManager.LoadScene(versusModeScene.Name, LoadSceneMode.Additive);
        Destroy(gameObject);
    }
    
    public void StartEndlessMode() {
        if (hasSelectedOption) return;
        hasSelectedOption = true;
        StartCoroutine(StartEnd());
    }
    
    private IEnumerator StartEnd() {
        animator.Play(startMode.name);
        
        yield return new WaitForSeconds(startMode.length);
        
        SceneManager.LoadScene(playerScene.Name);
        SceneManager.LoadScene(endlessModeScene.Name, LoadSceneMode.Additive);
        Destroy(gameObject);
    }
    
    public void SettingsMenu() {
        if (hasSelectedOption) return;
        hasSelectedOption = true;
        SceneManager.LoadScene(settingsScene.Name);
        Destroy(gameObject);
    }
    
    public void HowToPlayMenu() {
        if (hasSelectedOption) return;
        hasSelectedOption = true;
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
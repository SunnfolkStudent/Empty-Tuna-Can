using System.Collections;
using Audio;
using FMODUnity;
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

    [field: Header("Sound Clips")] 
    [field: SerializeField] private EventReference ButtonClick;
    
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
        AudioManager.Instance.PlayOneShot(ButtonClick, this.transform.position);
        AudioManager.Instance.StopMenuMusic();
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
        AudioManager.Instance.PlayOneShot(ButtonClick, this.transform.position);
        AudioManager.Instance.StopMenuMusic();
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
        AudioManager.Instance.PlayOneShot(ButtonClick, this.transform.position);
        AudioManager.Instance.StopMenuMusic();
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
        AudioManager.Instance.PlayOneShot(ButtonClick, this.transform.position);
        SceneManager.LoadScene(settingsScene.Name);
        Destroy(gameObject);
    }
    
    public void HowToPlayMenu() {
        if (hasSelectedOption) return;
        hasSelectedOption = true;
        AudioManager.Instance.PlayOneShot(ButtonClick, this.transform.position);
        SceneManager.LoadScene(howToPlayScene.Name);
        Destroy(gameObject);
    }
    
    public void QuitGame() {
        AudioManager.Instance.PlayOneShot(ButtonClick, this.transform.position);
        Application.Quit();
    }
    
    public void BackToMenu() {
        AudioManager.Instance.PlayOneShot(ButtonClick, this.transform.position);
        SceneManager.LoadScene(menuScene.Name);
        Destroy(gameObject);
    }   
}
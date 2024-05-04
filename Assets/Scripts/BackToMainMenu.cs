using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
public class BackToMainMenu : MonoBehaviour {
    [field: SerializeField] private EventReference ButtonClick;
    public void BackToMenu() {
        AudioManager.Instance.PlayOneShot(ButtonClick, this.transform.position);
        SceneManager.LoadScene("MainMenu");
        Destroy(gameObject);
    }
}
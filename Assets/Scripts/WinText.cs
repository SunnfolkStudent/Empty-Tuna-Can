using ModeManagers;
using TMPro;
using UnityEngine;

public class WinText : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text;
    
    private void Awake() {
        text.text = VersusModeManager.WinText;
    }
}

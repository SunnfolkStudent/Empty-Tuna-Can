using System.Collections;
using TMPro;
using UnityEngine;
using Utils.EventBus;

namespace Dialogue {
    public class DialogueUIManager : MonoBehaviour {
        [SerializeField] private float textSpeed = 0.1f;
        
        [Header("UIElements")]
        [SerializeField] private GameObject uiElement;
        [SerializeField] private TextMeshProUGUI nameField;
        [SerializeField] private TextMeshProUGUI textField;
    
        private EventBinding<DialogueEvent> playerEventBinding;

        private string _currentMessage;
    
        public static bool InDialog { get; private set; }
        public static bool DialogIsPlaying { get; private set; }
    
        private void OnEnable() {
            playerEventBinding = new EventBinding<DialogueEvent>(HandlePlayerEvent);
            EventBus<DialogueEvent>.Register(playerEventBinding);
            uiElement.SetActive(false);
        }
    
        private void OnDisable() {
            EventBus<DialogueEvent>.Deregister(playerEventBinding);
        }
    
        private void HandlePlayerEvent(DialogueEvent dialogueEvent) {
            switch (dialogueEvent) {
                case TextEvent @event:
                    StartDialog(@event);
                    break;
                case SkipEvent:
                    SkipDialog();
                    break;
                case EndEvent:
                    EndDialog();
                    break;
            }
        }
    
        private void StartDialog(TextEvent textEvent) {
            textField.text = "";
            nameField.text = "";
        
            InDialog = true;
            uiElement.SetActive(true);
        
            nameField.text = textEvent.Name;
            _currentMessage = textEvent.Text;
            StartCoroutine(WriteMessage());
        }

        private void SkipDialog() {
            if (!DialogIsPlaying) return;
        
            StopAllCoroutines();
            DialogIsPlaying = false;
            textField.text = _currentMessage;
        }

        private void EndDialog() {
            InDialog = false;
            uiElement.SetActive(false);
            nameField.text = "";
            textField.text = "";
            StopAllCoroutines();
        }
    
        private IEnumerator WriteMessage() {
            if (_currentMessage == null) yield break;
            DialogIsPlaying = true;
        
            var charArray = _currentMessage.ToCharArray();
        
            foreach (var c in charArray) {
                textField.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
        
            DialogIsPlaying = false;
        }
    }
}
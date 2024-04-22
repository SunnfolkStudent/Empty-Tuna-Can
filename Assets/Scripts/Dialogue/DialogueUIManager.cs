using System.Collections;
using TMPro;
using UnityEngine;
using Utils.EventBus;

namespace Dialogue {
    public class DialogueUIManager : MonoBehaviour {
        [SerializeField] [Tooltip("Characters per second")] private float defaultTextSpeed = 15f;
        
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
            switch (dialogueEvent.Event) {
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
            if (DialogIsPlaying) return;
            
            textField.text = "";
            nameField.text = "";
            
            InDialog = true;
            uiElement.SetActive(true);
            
            nameField.text = textEvent.name;
            _currentMessage = textEvent.text;
            StartCoroutine(WriteMessage(textEvent.textSpeed));
        }
        
        private void SkipDialog() {
            if (!DialogIsPlaying) return;
            
            StopAllCoroutines();
            DialogIsPlaying = false;
            textField.text = _currentMessage;
        }
        
        private void EndDialog() {
            StopAllCoroutines();
            
            InDialog = false;
            DialogIsPlaying = false;
            uiElement.SetActive(false);
            
            nameField.text = "";
            textField.text = "";
        }
        
        private IEnumerator WriteMessage(float textSpeed) {
            if (_currentMessage == null) yield break;
            DialogIsPlaying = true;
            if (textSpeed == 0) textSpeed = defaultTextSpeed;
            textSpeed = 1 / textSpeed;
            
            var charArray = _currentMessage.ToCharArray();
            
            foreach (var c in charArray) {
                textField.text += c;
                if (c == ' ') continue;
                yield return new WaitForSecondsRealtime(textSpeed);
            }
            
            DialogIsPlaying = false;
        }
    }
}
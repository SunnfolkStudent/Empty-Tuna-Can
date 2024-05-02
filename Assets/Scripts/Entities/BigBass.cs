using UnityEngine;
using Utils;

namespace Entities {
    public class BigBass : MonoBehaviour {
        [SerializeField] private RectTransform healthBar;
        [SerializeField] private Damageable damageable;

        private void Awake() {
            damageable.health.OnValueChanged += (value) => UpdateHealthBar(value, damageable.health.maxValue);
        }
        
        private void UpdateHealthBar(float value, float maxValue) {
            healthBar.localScale = healthBar.localScale.With(x: value / maxValue);
        }
    }
}

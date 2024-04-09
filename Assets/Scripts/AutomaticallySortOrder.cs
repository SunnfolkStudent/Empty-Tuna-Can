using UnityEngine;

public class AutomaticallySortOrder : MonoBehaviour {
    [SerializeField] private SpriteRenderer[] spriteRenderers;
    [SerializeField] private Transform objectTransform;

    private void Update() {
        foreach (var spriteRenderer in spriteRenderers) {
            spriteRenderer.sortingOrder = (int)(objectTransform.position.y * -64);
        }
    }
}
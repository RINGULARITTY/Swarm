using UnityEngine;

public class SelectableComponent {
    public bool IsSelected { get; private set; }
    private SpriteRenderer spriteRenderer;
    private Color defaultColor = Color.white;
    private Color selectedColor = Color.green;

    public SelectableComponent(SpriteRenderer renderer) {
        spriteRenderer = renderer;
    }

    public void Select() {
        IsSelected = true;
        spriteRenderer.color = selectedColor;
    }

    public void Deselect() {
        IsSelected = false;
        spriteRenderer.color = defaultColor;
    }
}

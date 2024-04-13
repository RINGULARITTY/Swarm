using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {
    public static SelectionManager Instance { get; private set; }
    public List<Entity> SelectedEntities { get; private set; } = new List<Entity>();
    private Camera mainCamera;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            mainCamera = Camera.main;
        } else {
            Destroy(this);
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null) {
                HandleEntitySelection(hit.collider);
            } else if (!(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) {
                DeselectAllEntities();
            }
        }
    }

    private void HandleEntitySelection(Collider2D collider) {
        EntityMonoLink monoLink = collider.GetComponent<EntityMonoLink>();
        if (monoLink != null && monoLink.Entity != null) {
            var groupSelectable = monoLink.Entity.GetComponent<GroupSelectableComponent>();
            if (groupSelectable != null && groupSelectable.IsGroupSelectable) {
                var selectable = monoLink.Entity.GetComponent<SelectableComponent>();
                if (selectable != null) {
                    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
                        if (selectable.IsSelected) {
                            selectable.Deselect();
                            SelectedEntities.Remove(monoLink.Entity);
                        } else {
                            selectable.Select();
                            SelectedEntities.Add(monoLink.Entity);
                        }
                    } else {
                        DeselectAllEntities();
                        selectable.Select();
                        SelectedEntities.Add(monoLink.Entity);
                    }
                }
            }
        }
    }

    private void DeselectAllEntities() {
        foreach (var entity in SelectedEntities) {
            entity.GetComponent<SelectableComponent>()?.Deselect();
        }
        SelectedEntities.Clear();
    }
}

using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {
    public static SelectionManager Instance { get; private set; }
    public List<Entity> SelectedEntities { get; private set; } = new List<Entity>();
    private Camera mainCamera;
    private Vector2 startMousePosition;
    private bool isSelecting = false;
    private LineRenderer selectionBoxLine;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            mainCamera = Camera.main;
            SetupSelectionBoxLine();
        } else {
            Destroy(this.gameObject);
        }
    }

    private void SetupSelectionBoxLine() {
        GameObject lineObj = new GameObject("SelectionBoxLine");
        selectionBoxLine = lineObj.AddComponent<LineRenderer>();
        selectionBoxLine.material = new Material(Shader.Find("Sprites/Default"));
        selectionBoxLine.startColor = Color.red;
        selectionBoxLine.endColor = Color.red;
        selectionBoxLine.startWidth = 0.05f;
        selectionBoxLine.endWidth = 0.05f;
        selectionBoxLine.positionCount = 5;
        selectionBoxLine.enabled = false;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            startMousePosition = Input.mousePosition;
            isSelecting = true;
            selectionBoxLine.enabled = true;
        }

        if (Input.GetMouseButton(0) && isSelecting) {
            UpdateSelectionBox(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0) && isSelecting) {
            ExecuteSelection();
            isSelecting = false;
            selectionBoxLine.enabled = false;
        }
    }

    private void UpdateSelectionBox(Vector2 currentMousePosition) {
        Vector3[] corners = new Vector3[5];
        corners[0] = mainCamera.ScreenToWorldPoint(new Vector3(startMousePosition.x, startMousePosition.y, 0f));
        corners[1] = mainCamera.ScreenToWorldPoint(new Vector3(currentMousePosition.x, startMousePosition.y, 0f));
        corners[2] = mainCamera.ScreenToWorldPoint(new Vector3(currentMousePosition.x, currentMousePosition.y, 0f));
        corners[3] = mainCamera.ScreenToWorldPoint(new Vector3(startMousePosition.x, currentMousePosition.y, 0f));
        corners[4] = corners[0];

        for (int i = 0; i < 5; i++) {
            corners[i].z = 0;
        }

        selectionBoxLine.SetPositions(corners);
    }

    private void ExecuteSelection() {
        Vector2 min = mainCamera.ScreenToWorldPoint(new Vector3(Mathf.Min(startMousePosition.x, Input.mousePosition.x), Mathf.Min(startMousePosition.y, Input.mousePosition.y), 0));
        Vector2 max = mainCamera.ScreenToWorldPoint(new Vector3(Mathf.Max(startMousePosition.x, Input.mousePosition.x), Mathf.Max(startMousePosition.y, Input.mousePosition.y), 0));
        Rect selectionRect = new Rect(min.x, min.y, max.x - min.x, max.y - min.y);

        Collider2D[] colliders = Physics2D.OverlapAreaAll(min, max);

        bool isControlPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        if (!isControlPressed) {
            DeselectAllEntities();
        }

        foreach (Collider2D collider in colliders) {
            HandleEntitySelection(collider.gameObject, isControlPressed);
        }
    }

    private void HandleEntitySelection(GameObject obj, bool isControlPressed) {
        EntityMonoLink monoLink = obj.GetComponent<EntityMonoLink>();
        if (monoLink != null && monoLink.Entity != null) {
            GroupSelectableComponent groupSelectable = monoLink.Entity.GetComponent<GroupSelectableComponent>();
            if (groupSelectable != null && groupSelectable.IsGroupSelectable) {
                SelectableComponent selectable = monoLink.Entity.GetComponent<SelectableComponent>();
                if (selectable != null) {
                    if (isControlPressed) {
                        ToggleEntitySelection(selectable, monoLink.Entity);
                    } else {
                        selectable.Select();
                        SelectedEntities.Add(monoLink.Entity);
                    }
                }
            }
        }
    }

    private void ToggleEntitySelection(SelectableComponent selectable, Entity entity) {
        if (selectable.IsSelected) {
            selectable.Deselect();
            SelectedEntities.Remove(entity);
        } else {
            selectable.Select();
            SelectedEntities.Add(entity);
        }
    }

    private void DeselectAllEntities() {
        foreach (var entity in SelectedEntities) {
            entity.GetComponent<SelectableComponent>()?.Deselect();
        }
        SelectedEntities.Clear();
    }
}
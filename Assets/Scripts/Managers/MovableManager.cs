using UnityEngine;

public class MovableManager : MonoBehaviour {
    private Camera mainCamera;

    private void Awake() {
        mainCamera = Camera.main;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1))  // Clic droit
        {
            Vector3 targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0;  // Assurez-vous que la position z est correcte pour les entit�s 2D

            foreach (Entity entity in SelectionManager.Instance.SelectedEntities) {
                var movable = entity.GetComponent<MovableComponent>();
                var selectable = entity.GetComponent<SelectableComponent>();
                if (selectable != null && selectable.IsSelected && movable != null) {
                    movable.TargetPosition = targetPosition;
                    movable.IsMoving = true;
                    selectable.Deselect();  // D�s�lectionne l'entit� apr�s le d�but du mouvement
                }
            }
        }
    }
}

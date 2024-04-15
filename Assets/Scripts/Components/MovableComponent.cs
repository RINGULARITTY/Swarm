using UnityEngine;

public class MovableComponent {
    public float Speed { get; private set; }
    public Vector3 TargetPosition { get; set; }
    public bool IsMoving { get; set; }

    public MovableComponent(float speed) {
        Speed = speed;
        IsMoving = false;
    }

    public void MoveTowardsTarget(Transform transform) {
        if (IsMoving) {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, Speed * Time.deltaTime);
            if (transform.position == TargetPosition) {
                IsMoving = false;
            }
        }
    }
}
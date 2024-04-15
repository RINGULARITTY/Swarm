using UnityEngine;

public class EntityMonoLink : MonoBehaviour {
    public Entity Entity { get; set; }

    private void Update() {
        MovableComponent movable = Entity.GetComponent<MovableComponent>();
        if (movable != null) {
            movable.MoveTowardsTarget(transform);
        }
    }
}
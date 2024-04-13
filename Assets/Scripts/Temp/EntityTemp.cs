using UnityEngine;

public class EntitySpawner : MonoBehaviour {
    public GameObject entityPrefab;
    public int numberOfEntities = 10;

    private void Start() {
        for (int i = 0; i < numberOfEntities; i++) {
            GameObject entityGO = Instantiate(entityPrefab, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0), Quaternion.identity);
            Entity entity = new Entity(i);
            entity.AddComponent(new StatsComponent(100, 15));
            entity.AddComponent(new SelectableComponent(entityGO.GetComponent<SpriteRenderer>()));
            entity.AddComponent(new GroupSelectableComponent());
            entityGO.GetComponent<EntityMonoLink>().Entity = entity;
        }
    }
}

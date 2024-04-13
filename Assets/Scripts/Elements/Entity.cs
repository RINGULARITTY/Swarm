using System.Collections.Generic;

public class Entity {
    public int ID { get; private set; }
    private Dictionary<System.Type, object> components;

    public Entity(int id) {
        ID = id;
        components = new Dictionary<System.Type, object>();
    }

    public void AddComponent<T>(T component) where T : class {
        components[typeof(T)] = component;
    }

    public T GetComponent<T>() where T : class {
        if (components.TryGetValue(typeof(T), out object component)) {
            return component as T;
        }
        return null;
    }
}
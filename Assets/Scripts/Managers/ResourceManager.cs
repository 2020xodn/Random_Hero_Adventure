using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager 
{
    public T Load<T>(string path) where T : Object {
        if (typeof(T) == typeof(GameObject)) {
            string name = path;

            int index = name.LastIndexOf("/");
            if (index >= 0)
                name = name.Substring(index + 1);

            GameObject go = Managers.Pool.GetOriginal(name);

            if (go != null) return go as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null) {
        GameObject original = Load<GameObject>($"Prefabs/{path}");

        if (original == null) {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        // Object Pool Check
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;
        else {
            GameObject go = Object.Instantiate(original, parent);
            go.name = original.name;

            return go;
        }
    }

    public GameObject Instantiate(GameObject original, Vector3 pos, Quaternion rot) {
        if (original == null) {
            return null;
        }

        // Object Pool Check
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original.gameObject).gameObject;
        else {
            GameObject go = Object.Instantiate(original.gameObject);
            go.name = original.name;
            go.transform.position = pos;
            go.transform.rotation = rot;
            return go;
        }
    }

    public void Destroy(GameObject go) {
        if (go == null) return;

        // Object Pool Check -> Not Destroy
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null) {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}
